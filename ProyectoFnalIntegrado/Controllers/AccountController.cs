using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace ProyectoFnalIntegrado.Controllers
{
    public class AccountController : Controller
    {
        private readonly DaContext _context;
        private readonly EmailService _emailService;
        private readonly string _secretKey = "TuClaveSecretaParaToken";

        public AccountController(DaContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

       

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //var user = _context.Userrs.FirstOrDefault(u => u.UserName == username);
            //if (user != null && HashHelper.VerifyPassword(password, user.Password))
            //{
            //    var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Role, user.Rol)
            //};

            //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var authProperties = new AuthenticationProperties
            //    {
            //        IsPersistent = true,
            //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            //    };

            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
            //    return View();
            //}


            var user = _context.Userrs.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                // Guardar rol e ID del usuario en variables de sesión
                HttpContext.Session.SetString("UserRole", user.Rol);
                HttpContext.Session.SetInt32("UserId", user.Id);

                int? userId = HttpContext.Session.GetInt32("UserId");
                string? userRole = HttpContext.Session.GetString("UserRole");

                if (userId == null || string.IsNullOrEmpty(userRole))
                {
                    return Unauthorized("El usuario no está autenticado.");
                }

                if (userRole == "Enfermero" || userRole == "Director" )
                {
                    // Buscar el idSchool solo para roles específicos
                    var assignment = _context.Assignments
                        .Include(a => a.IdNurseNavigation)
                        .FirstOrDefault(a => a.IdNurseNavigation.Id == userId);

                    if (assignment != null)
                    {
                        var idSchool = assignment.IdSchool;
                        HttpContext.Session.SetInt32("IdEstablisment", idSchool);
                    }
                    else
                    {
                        return NotFound("No se encontró una asignación para este usuario.");
                    }
                }

                // Crear los claims para el usuario autenticado
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var nurse = _context.Nurses.FirstOrDefault(n => n.Email == email);
            if (nurse == null)
            {
                ViewBag.ErrorMessage = "No se encontró una cuenta con ese correo.";
                return View();
            }

            var user = _context.Userrs.FirstOrDefault(u => u.Id == nurse.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "No se encontró un usuario asociado a esta cuenta.";
                return View();
            }

            var token = GenerateResetToken(user.UserName);
            var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);

            // Enviar el enlace de restablecimiento por correo electrónico
            string subject = "Recuperación de Contraseña";
            string body = $"Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href='{resetLink}'>Restablecer Contraseña</a>";

            await _emailService.SendEmailAsync(email, subject, body);
            ViewBag.Message = "Se ha enviado un enlace de recuperación de contraseña a tu correo electrónico.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token) || !IsTokenValid(token, out _))
            {
                ViewBag.ErrorMessage = "El enlace de restablecimiento ha expirado o es inválido.";
                return RedirectToAction("ForgotPassword");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newPassword)
        {
            if (!IsTokenValid(token, out string username))
            {
                ViewBag.ErrorMessage = "El enlace de restablecimiento ha expirado o es inválido.";
                return View();
            }

            var user = _context.Userrs.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Usuario no encontrado.";
                return View();
            }

            user.Password = HashHelper.ComputeSha256Hash(newPassword);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Tu contraseña ha sido restablecida exitosamente.";
            return RedirectToAction("Login");
        }

        private string GenerateResetToken(string username)
        {
            var expiration = DateTime.UtcNow.AddMinutes(15);
            var payload = $"{username}|{expiration:O}";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var keyBytes = Encoding.UTF8.GetBytes(_secretKey);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hash = hmac.ComputeHash(payloadBytes);
                var token = $"{Convert.ToBase64String(payloadBytes)}:{Convert.ToBase64String(hash)}";
                return token;
            }
        }

        private bool IsTokenValid(string token)
        {
            return IsTokenValid(token, out _);
        }

        private bool IsTokenValid(string token, out string username)
        {
            username = null;
            try
            {
                var parts = token.Split(':');
                if (parts.Length != 2) return false;

                var payloadBytes = Convert.FromBase64String(parts[0]);
                var hashBytes = Convert.FromBase64String(parts[1]);
                var keyBytes = Encoding.UTF8.GetBytes(_secretKey);

                using (var hmac = new HMACSHA256(keyBytes))
                {
                    var computedHash = hmac.ComputeHash(payloadBytes);
                    if (!computedHash.SequenceEqual(hashBytes)) return false;
                }

                var payload = Encoding.UTF8.GetString(payloadBytes);
                var segments = payload.Split('|');
                if (segments.Length != 2) return false;

                username = segments[0];
                var expiration = DateTime.Parse(segments[1]);

                return expiration > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

     
    }
}
