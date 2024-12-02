using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramColegioEnfermeras.Models;
using System.Threading.Tasks;

namespace ProgramColegioEnfermeras.Controllers
{
    public class AccountController : Controller
    {
        private readonly BdUnivalleEnfermerasContext _context;

        public AccountController(BdUnivalleEnfermerasContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                // Buscar el usuario por el nombre de usuario
                var user = await _context.Userrs.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    // Si no se encuentra el usuario, usar ViewBag para mostrar un mensaje de error
                    ViewBag.ErrorMessage = "Usuario no encontrado.";
                    return View();
                }

                // Comparar las contraseñas (la ingresada vs la almacenada)
                if (HashHelper.VerifyPassword(password, user.Password))
                {
                    // Si coinciden, redirigir al área protegida o al dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Si no coinciden, usar ViewBag para mostrar un mensaje de error
                    ViewBag.ErrorMessage = "Contraseña incorrecta.";
                    return View();
                }
            }

            return View();
        }
    }
}
