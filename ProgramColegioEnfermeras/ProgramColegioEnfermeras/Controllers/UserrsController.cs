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

    public class UserrsController : Controller
    {
        private readonly BdUnivalleEnfermerasContext _context;

        public UserrsController(BdUnivalleEnfermerasContext context)
        {
            _context = context;
        }

        // GET: Userrs
        public async Task<IActionResult> Index()
        {
            var bdUnivalleEnfermerasContext = _context.Userrs.Include(u => u.IdNavigation);
            return View(await bdUnivalleEnfermerasContext.ToListAsync());
        }

        // GET: Userrs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.IdNavigation) // Incluye la enfermera relacionada
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userr == null)
            {
                // Redirige al método Create con el id de la enfermera
                return RedirectToAction("Create", new { id });
            }

            return View(userr);
        }

        // GET: Userrs/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Crear un nuevo usuario con el ID de la enfermera preasignado
            var userr = new Userr
            {
                Id = id.Value, // Asignar el ID de la enfermera al nuevo usuario
                RegistrationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Id", id);
            return View(userr); // Pasamos el nuevo usuario a la vista
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Rol,State")] Userr userr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si la enfermera existe.
                    var nurse = await _context.Nurses.FindAsync(userr.Id);
                    if (nurse == null)
                    {
                        ModelState.AddModelError("", "No existe una enfermera con ese ID.");
                        return View(userr);
                    }

                    // Encriptar la contraseña antes de guardar.
                    userr.Password = HashHelper.ComputeSha256Hash(userr.Password);

                    userr.RegistrationDate = DateTime.Now;

                    _context.Add(userr);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Id", userr.Id);
            return View(userr);
        }



        // GET: Userrs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Id", userr.Id);

            return View(userr);
        }

        // POST: Userrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Rol,RegistrationDate,UpdateDate,State")] Userr userr)
        {
            if (id != userr.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["Id"] = new SelectList(_context.Nurses, "Id", "Id", userr.Id);
            return View(userr);
        }

        // GET: Userrs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
            var userr = await _context.Userrs.FindAsync(id);
            if (userr != null)
            {
                _context.Userrs.Remove(userr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserrExists(int id)
        {
            return _context.Userrs.Any(e => e.Id == id);
        }
    }

}
