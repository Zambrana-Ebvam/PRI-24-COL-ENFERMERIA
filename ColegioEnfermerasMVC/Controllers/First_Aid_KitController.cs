using ColegioEnfermerasMVC.Data;
using ColegioEnfermerasMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Controllers
{
    public class First_Aid_KitController : Controller
    {
        private readonly DbCollegeContext db;

        public First_Aid_KitController()
        {
            db = new DbCollegeContext();
        }


        // GET: First_Aid_KitController/Create
        public ActionResult CreateFirst()
        {
            return View();
        }

        // GET: Recuperar todos las escuelas activas
        [HttpGet]
        public ActionResult<List<object>> GetActiveSchools()
        {
            try
            {
                var suppliers = db.Schools
                                  .Where(s => s.Status == 1)
                                  .Select(s => new
                                  {
                                      s.Id,
                                      s.Name
                                  })
                                  .ToList();

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar proveedores: {ex.Message}");
            }
        }

        // GET: Recuperar todos los responsables activas
        [HttpGet]
        public ActionResult<List<object>> GetNursesBySchool(int schoolId)
        {
            //GET / First_Aid_Kit / GetNursesBySchool ? schoolId = 1
            try
            {

                var nurses = db.Assignments
                    .Where(a => a.IdSchool == schoolId && a.Status == 1)
                    .Join(db.Nurses,
                          a => a.IdNurse,
                          n => n.Id,
                          (a, n) => n)
                    .Join(db.People,
                          n => n.Id,
                          p => p.Id,
                          (n, p) => new
                          {
                              NurseId = n.Id,
                              FullName = $"{p.Names} {p.Surname}"
                          })
                    .ToList();


                return Ok(nurses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los enfermeros asignados: {ex.Message}");
            }
        }
        //Lista Medicamentos
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetActiveMedicines()
        {
            try
            {

                var activeMedicines = await db.Medicines
                    .Where(m => m.Status == 1)
                    .Select(m => new
                    {
                        MedicineId = m.Id,
                        MedicineName = m.Name,
                        MedicineDescription = m.Description,
                        MedicineStock = m.Stock
                    })
                    .ToListAsync();



                return Ok(activeMedicines);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los medicamentos activos: {ex.Message}");
            }
        }

        public class FirstAidKitWithMedicinesDTO
        {
            public FirstAidKit FirstAidKit { get; set; }
            public List<MedicineFirstAidKitDTO> Medicines { get; set; }
        }

        public IActionResult CreateFirstAidKit([FromBody] FirstAidKitWithMedicinesDTO dto)
        {
       
            using var transaction = db.Database.BeginTransaction();
            try
            {
                
                dto.FirstAidKit.UserId = 1;
                dto.FirstAidKit.RegisterDate = DateTime.Now;
                db.FirstAidKits.Add(dto.FirstAidKit);
                db.SaveChanges();

          
                var firstAidKitId = dto.FirstAidKit.Id;

                foreach (var medicine in dto.Medicines)
                {
                  
                    var existsInDb = db.Medicines.Any(m => m.Id == medicine.IdMedicine);
                    if (!existsInDb)
                    {
                       
                        Console.WriteLine($"Alerta: El medicamento con Id {medicine.IdMedicine} no existe en la base de datos y no será añadido al botiquín.");
                        continue;
                    }

                    
                    var existingMedicine = db.MedicineFirstAidKits
                        .FirstOrDefault(m => m.IdAid == firstAidKitId && m.IdMedicine == medicine.IdMedicine);

                    if (existingMedicine != null)
                    {
                     
                        existingMedicine.Quantity += medicine.Quantity;
                        existingMedicine.LastUpdated = DateTime.Now;
                    }
                    else
                    {
                      
                        var newMedicine = new MedicineFirstAidKit
                        {
                            IdAid = firstAidKitId,
                            IdMedicine = medicine.IdMedicine,
                            Quantity = medicine.Quantity,
                            ExpirationDate = medicine.ExpirationDate ?? DateTime.Now.AddYears(1),
                            LastUpdated = DateTime.Now
                        };
                        db.MedicineFirstAidKits.Add(newMedicine);
                    }
                }

                db.SaveChanges();
                transaction.Commit();
                return Ok("Botiquín y medicamentos insertados correctamente.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Error al crear el botiquín: {innerException}");
            }
        }


        public class MedicineFirstAidKitDTO
        {
            public short IdMedicine { get; set; }
            public int Quantity { get; set; }
            public DateTime? ExpirationDate { get; set; } // Este campo es opcional
        }
    }
}
