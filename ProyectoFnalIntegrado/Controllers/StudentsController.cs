using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;



namespace ProyectoFnalIntegrado.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DaContext _context;

        public StudentsController(DaContext context)
        {
            _context = context;
        }

        
        // GET: Students
        public async Task<IActionResult> Index()
        {
           
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");

                // Filtra los estudiantes por el IdEstablishment
                var filteredStudents = _context.Students
                    .Include(s => s.IdEstablishmentNavigation)
                    .Where(s => s.IdEstablishment == idEstablishment);

                return View(await filteredStudents.ToListAsync());
            

            // Si no existe el IdEstablishment en la sesión, muestra todos los estudiantes
            var allStudents = _context.Students.Include(s => s.IdEstablishmentNavigation);
            return View(await allStudents.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.IdEstablishmentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            // Obtén la lista de establecimientos y envíala al ViewBag
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Names,FirstName,MiddleName,Gender,Birthdate,Code,Tutor,TutorCellphone,BloodType,Allergy,RegistrationDate,UpdateDate,State,IdEstablishment")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Name");
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Cargar establecimientos para el combo
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Name", student.IdEstablishment);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Names,FirstName,MiddleName,Gender,Birthdate,Code,Tutor,TutorCellphone,BloodType,Allergy,RegistrationDate,UpdateDate,State,IdEstablishment")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (!student.UpdateDate.HasValue)
                    {
                        student.UpdateDate = DateTime.Now;
                    }


                    _context.Update(student);

                    await _context.SaveChangesAsync();
                }

                catch (Exception ex)
                {
                    // Capturar cualquier otra excepción y mostrar el mensaje
                    Debug.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "Hubo un error al actualizar los datos.");
                }

                ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Name", student.IdEstablishment);
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Name", student.IdEstablishment);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.IdEstablishmentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost]
        public IActionResult Deactivate(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return Json(new { success = false, message = "Estudiante no encontrado." });
            }

            // Lógica para desactivar (por ejemplo, marcar un campo como inactivo)
            student.State = 0;
            _context.SaveChanges();

            return Json(new { success = true, message = "Estudiante desactivado exitosamente." });
        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
