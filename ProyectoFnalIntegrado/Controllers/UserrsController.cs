using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using ProyectoFnalIntegrado.Services;


namespace ProyectoFnalIntegrado.Controllers
{
    public class UserrsController : Controller
    {
        private readonly DaContext _context;
        private readonly EmailService emailService;
        public UserrsController(DaContext context)
        {
            _context = context;
        }

        // Método privado para verificar si el usuario es Admin
        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        // Método privado para verificar si el usuario es Director
        private bool IsDirector()
        {
            return User.IsInRole("Director");
        }


        // GET: Userrs
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Filtrar solo usuarios con State = 1
            var bdUnivalleEnfermerasContext = _context.Userrs
                .Include(u => u.IdNavigation)
                .Where(u => u.State == 1);

            return View(await bdUnivalleEnfermerasContext.ToListAsync());
        }

        // GET: Userrs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsAdmin() && !IsDirector())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userr == null)
            {
                return NotFound();
            }

            return View(userr);
        }

        // GET: Userrs/Create
        public IActionResult Create(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var userr = new Userr
            {
                Id = id.Value,
                RegistrationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Rol = "Enfermero"  // Por defecto, el rol será "Enfermero"
            };

            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Names", id);
            return View(userr);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Rol,State")] Userr userr)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nurse = await _context.Nurses.FindAsync(userr.Id);
                    if (nurse == null)
                    {
                        ModelState.AddModelError("", "No existe una enfermera con ese ID.");
                        return View(userr);
                    }

                    //userr.Password = HashHelper.ComputeSha256Hash(userr.Password);

                    userr.Password = "t$PaTd9c";
                    userr.RegistrationDate = DateTime.Now;
                    emailService.SendCredentialsAsync(nurse.Email, userr.UserName, userr.Password);
                    _context.Add(userr);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Names", userr.Id);
            return View(userr);
        }

        // GET: Userrs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Names", userr.Id);
            return View(userr);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Rol,RegistrationDate,UpdateDate,State")] Userr userr)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id != userr.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Userrs.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Verificar si la contraseña ha cambiado y aplicar hash si es necesario
                    if (existingUser.Password != userr.Password)
                    {
                        userr.Password = HashHelper.ComputeSha256Hash(userr.Password);
                    }

                    userr.UpdateDate = DateTime.Now;

                    _context.Update(userr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserrExists(userr.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Names", userr.Id);
            return View(userr);
        }


        [HttpPost]
        public async Task<IActionResult> EditRol(int id, [Bind("Id,Rol")] Userr userr)
        {
            //if (!IsAdmin())
            //{
            //    return Json(new { success = false, message = "Acceso denegado." });
            //}

            //if (id != userr.Id)
            //{
            //    return Json(new { success = false, message = "Usuario no encontrado." });
            //}

           
                try
                {
                    var existingUser = await _context.Userrs.FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser == null)
                    {
                        return Json(new { success = false, message = "Usuario no encontrado." });
                    }

                    existingUser.Rol = userr.Rol;
                    existingUser.UpdateDate = DateTime.Now;

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Error al actualizar el rol." });
                }
            

          
        }


        // GET: Userrs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userr == null)
            {
                return NotFound();
            }

            return View(userr);
        }

        // POST: Userrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr != null)
            {
                _context.Userrs.Remove(userr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Deactivate(int id)
        {
            var user = _context.Userrs.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            // Lógica para desactivar (por ejemplo, marcar un campo como inactivo)
            user.State = 0;
            _context.SaveChanges();

            return Json(new { success = true, message = "Usuario desactivado exitosamente." });
        }

        private bool UserrExists(int id)
        {
            return _context.Userrs.Any(e => e.Id == id);
        }
    }
    public static class HashHelper
    {
       
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // 40 caracteres
            }
        }

    
        public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            
            var enteredPasswordHash = ComputeSha256Hash(enteredPassword);

          
            return enteredPasswordHash == storedPasswordHash;
        }
    }
}
