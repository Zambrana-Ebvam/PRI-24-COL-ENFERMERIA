using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgramColegioEnfermeras.Models;

namespace ProgramColegioEnfermeras.Controllers
{
    public class StudentsController : Controller
    {
        private readonly BdUnivalleEnfermerasContext _context;

        public StudentsController(BdUnivalleEnfermerasContext context)
        {
            _context = context;
        }

        // GET: Students
        // Index con búsqueda dinámica
        public async Task<IActionResult> Index(string? search)
        {
            var students = _context.Students
                .Include(s => s.IdEstablishmentNavigation) // Incluye navegación del establecimiento

                // Filtra por los campos especificados si se proporciona un valor de búsqueda
                .Where(s =>
                    (string.IsNullOrEmpty(search) || s.Names.Contains(search)) ||
                    (string.IsNullOrEmpty(search) || s.FirstName.Contains(search)) ||
                    (string.IsNullOrEmpty(search) || (s.MiddleName != null && s.MiddleName.Contains(search))) ||
                    (string.IsNullOrEmpty(search) || s.Code.Contains(search))
                );

            return View(await students.ToListAsync());
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
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Id");
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
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Id", student.IdEstablishment);
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
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Id", student.IdEstablishment);
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
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["IdEstablishment"] = new SelectList(_context.Establishments, "Id", "Id", student.IdEstablishment);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
