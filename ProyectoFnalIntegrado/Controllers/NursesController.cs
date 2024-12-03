using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using ProyectoFnalIntegrado.Services;
using System.Diagnostics;


namespace ProyectoFnalIntegrado.Controllers
{
    public class NursesController : Controller
    {
        private readonly DaContext _context;
       
        public NursesController(DaContext context)
        {
            _context = context;
            
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        private bool IsEnfermero()
        {
            return User.IsInRole("Enfermero");
        }


        // GET: NursesController
        public async Task<ActionResult> IndexAsync()
        {
            
            var nurses = await _context.Nurses
                .Where(n => n.State == 1) 
                .ToListAsync();

            return View(nurses);  
        }


        // GET: NursesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NursesController/Create
        public ActionResult Create()
        {
            if (!IsAdmin() && !IsEnfermero())
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            return View();
        }

        // POST: NursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Names,FirstName,MiddleName,Gender,Birthdate,Ci,Cellphone,Email,Address,CodeSedes,Specialty")] Nurse nurse, string Rol)
        {
            if (ModelState.IsValid)
            {
               
                _context.Nurses.Add(nurse);
                await _context.SaveChangesAsync();

              
                int nurseId = nurse.Id;

          
                string username = $"{nurse.Names.Substring(0, 1).ToLower()}{nurse.FirstName.Substring(0, 1).ToLower()}.{Rol.ToLower()}";

                string passwordOld = "Password123";
                var user = new Userr
                { 
                    Id = nurseId,
                    UserName = username,
                    Password = passwordOld,
                    Rol = Rol,
                    RegistrationDate = DateTime.Now,
                    State = 1, 
                    
                };

                
                _context.Userrs.Add(user);
                await _context.SaveChangesAsync();

                var emailService = new EmailService();
                await emailService.SendCredentialsAsync(nurse.Email, username, passwordOld);

                return RedirectToAction(nameof(Index));
            }
            return View(nurse);
        }



        // GET: NursesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            if (id == null || (!IsAdmin() && !IsEnfermero()))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }
            return View(nurse);
        }

        // POST: NursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, [Bind("Id,Names,FirstName,MiddleName,Gender,Birthdate,Ci,Cellphone,Email,Address,CodeSedes,Specialty")] Nurse nurse)
        {
            // Verificar si el id coincide o si el usuario tiene permisos adecuados
            if (id != nurse.Id || (!IsAdmin() && !IsEnfermero()))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

           
            if (ModelState.IsValid)
            {
                try
                {

                    if (!nurse.UpdateDate.HasValue)
                    {
                        nurse.UpdateDate = DateTime.Now;
                    }


                    _context.Update(nurse);

                    await _context.SaveChangesAsync();
                }
           
                catch (Exception ex)
                {
                    // Capturar cualquier otra excepción y mostrar el mensaje
                    Debug.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "Hubo un error al actualizar los datos.");
                }

                return RedirectToAction(nameof(Index));
            }

          
            return View(nurse);
        }

     
      




        // GET: NursesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id == null || !IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var nurse = await _context.Nurses.FirstOrDefaultAsync(m => m.Id == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // POST: NursesController/Deactivate/5
        // POST: NursesController/Deactivate/5
        [HttpPost]
        public async Task<ActionResult> Deactivate(int id)
        {
            // Verifica si el usuario tiene permisos para realizar la acción
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Obtén el enfermero por su ID
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return BadRequest("Enfermero no encontrado");
            }

            // Cambia el estado del enfermero a desactivado (estado 0)
            nurse.State = 0;
            _context.Nurses.Update(nurse);
            await _context.SaveChangesAsync();

       
            return Json(new { success = true, message = "Enfermero desactivado correctamente." });
        }



        private bool NurseExists(int id)
        {
            return _context.Nurses.Any(e => e.Id == id);
        }
    }
}
