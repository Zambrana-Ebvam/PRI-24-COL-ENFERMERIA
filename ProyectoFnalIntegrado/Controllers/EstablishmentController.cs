
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;


namespace ProyectoFnalIntegrado.Controllers
{
    public class EstablishmentController : Controller
    {
        private readonly DaContext _context;

        public EstablishmentController(DaContext context)
        {
            _context = context;
        }

        public IActionResult CRUD()
        {

            ViewBag.Provinces = new SelectList(_context.Provinces.ToList(), "Id", "Name");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllEstablishments()
        {
            var establishments = await _context.Establishments
                .Include(e => e.Province)
                .Where(e => e.Status == 1)
                .Select(e => new {
                    id = e.Id,
                    name = e.Name,
                    schedule = e.Schedule,
                    zone = e.Zone,
                    provinceName = e.Province.Name
                }).ToListAsync();

            return Json(establishments);

        }


        public IActionResult Create()
        {
            ViewBag.Provinces = new SelectList(_context.Provinces, "Id", "Name");
            return View();
        }




        [HttpPost]
        public IActionResult Create(Establishment establishment)
        {
            if (ModelState.IsValid)
            {
                establishment.RegisterDate = DateTime.Now;
                establishment.Status = 1;
                _context.Establishments.Add(establishment);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                var errorList = ModelState.Values.SelectMany(v => v.Errors)
                                                 .Select(e => e.ErrorMessage)
                                                 .ToList();
                return Json(new { success = false, message = "Datos no válidos.", errors = errorList });
            }
        }











        [HttpGet]
        public IActionResult Edit(byte id)
        {
            var establishment = _context.Establishments.Find(id);
            if (establishment == null)
            {
                return NotFound();
            }
            return Json(establishment);
        }

        [HttpPost]
        public IActionResult Edit(Establishment establishment)
        {
            if (ModelState.IsValid)
            {
                var existingEstablishment = _context.Establishments.Find(establishment.Id);
                if (existingEstablishment == null)
                {
                    return Json(new { success = false, message = "Establecimiento no encontrado." });
                }


                existingEstablishment.Name = establishment.Name;
                existingEstablishment.Schedule = establishment.Schedule;
                existingEstablishment.Zone = establishment.Zone;
                existingEstablishment.Latitude = establishment.Latitude;
                existingEstablishment.Longitude = establishment.Longitude;
                existingEstablishment.Phone = establishment.Phone;
                existingEstablishment.Director = establishment.Director;
                existingEstablishment.ProvinceId = establishment.ProvinceId;
                existingEstablishment.LastUpdate = DateTime.Now;

                _context.SaveChanges();
                return Json(new { success = true, message = "Establecimiento actualizado correctamente." });
            }

            return Json(new { success = false, message = "Datos no válidos." });
        }


        [HttpPost]
        public IActionResult Delete(byte id)
        {
            var establishment = _context.Establishments.Find(id);
            if (establishment == null)
            {
                return NotFound();
            }

            establishment.Status = 0;
            _context.Establishments.Update(establishment);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}