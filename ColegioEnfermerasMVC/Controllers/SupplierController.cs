using ColegioEnfermerasMVC.Data;
using ColegioEnfermerasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ColegioEnfermerasMVC.Controllers
{
    public class SupplierController : Controller
    {
        private readonly DbCollegeContext db;

        public SupplierController()
        {
            db = new DbCollegeContext();
        }

        public ActionResult CreateSupplier()
        {
            return View();
        }

        public ActionResult FillStock()
        {
            return View();
        }

        // GET: Recuperar todos los proveedores activos
        [HttpGet]
        public ActionResult<List<object>> GetActiveSuppliers()
        {
            try
            {
                var suppliers = db.Suppliers
                                  .Where(s => s.Status == 1)
                                  .OrderByDescending(s => s.Id) 
                                  .Select(s => new
                                  {
                                      s.Id,
                                      s.Name,
                                      s.Adress,
                                      s.Phone,
                                      RegisterDate = s.RegisterDate.ToString("yyyy-MM-dd"),
                                      s.LastUpdate,
                                      s.UserId,
                                      s.Status
                                  })
                                  .ToList();

                return Ok(suppliers);  // Devolver la lista directamente
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar proveedores: {ex.Message}");
            }
        }



        // POST: Insertar un nuevo proveedor
        [HttpPost]
        public IActionResult InsertSupplier([FromBody] Supplier supplier)
        {
            try
            {
               
                Console.WriteLine(supplier);

                var newSupplier = new Supplier
                {
                    Name = supplier.Name,
                    Adress = supplier.Adress,
                    Phone = supplier.Phone,
                    RegisterDate = DateTime.Now,
                    UserId = supplier.UserId,
                    Status = 1
                };

                db.Suppliers.Add(newSupplier);
                db.SaveChanges();

                return Ok("Proveedor creado exitosamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al crear el proveedor: {ex.Message} - Detalles: {ex.InnerException?.Message}";
                return BadRequest(errorMessage);
            }
        }


        // Actualizar un proveedor
        [HttpPost]
        public IActionResult UpdateSupplier([FromBody] Supplier supplier)
        {
            try
            {
                // Cargar la entidad existente desde la base de datos
                var supplierup = db.Suppliers.Find(supplier.Id);
                if (supplierup == null)
                {
                    return NotFound("Proveedor no encontrado.");
                }

                // Actualizar las propiedades de la entidad cargada
                supplierup.Name = supplier.Name;
                supplierup.Adress = supplier.Adress;
                supplierup.Phone = supplier.Phone;
                supplierup.Status = 1;
                supplierup.LastUpdate = DateTime.Now;

                // Guardar cambios
                db.SaveChanges();

                return Ok("Proveedor actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el proveedor: {ex.Message}");
            }
        }

        //Eliminar o desactivar un proveedor
        [HttpPost]
        public IActionResult DeleteSupplier(byte id)
        {
            try
            {
                var supplier = db.Suppliers.Find(id);
                if (supplier == null)
                {
                    return NotFound("Proveedor no encontrado.");
                }

                supplier.Status = 0;
                supplier.LastUpdate = DateTime.Now; 

                db.Suppliers.Update(supplier);
                db.SaveChanges();

                return Ok("Proveedor desactivado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al desactivar el proveedor: {ex.Message}");
            }
        }

        // POST: Obtener los medicamentos de un proveedor por su ID

        [HttpPost]
        public IActionResult GetMedicinesBySupplierId(byte id)
        {
            try
            {
                Console.WriteLine($"Supplier ID recibido: {id}");

                var medicines = db.Medicines
                                  .Where(m => m.SupplierId == id)
                                  .Select(m => new
                                  {
                                      m.Id,
                                      m.Name,
                                      m.Stock
                                  })
                                  .ToList();

                Console.WriteLine($"Número de medicamentos encontrados: {medicines.Count}");

                if (!medicines.Any())
                {
                    return NotFound("No se encontraron medicamentos asociados a este proveedor.");
                }

                return Ok(medicines);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los medicamentos: {ex.Message}");
            }
        }

        // GET: Recuperar todos los proveedores activos que tienen medicamentos
        [HttpGet]
        public ActionResult<List<object>> GetActiveSuppliersMedicine()
        {
            try
            {
                var suppliers = db.Suppliers
                                  .Where(s => s.Status == 1 && s.Medicines.Any()) // Filtra proveedores activos que tengan medicamentos
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

        [HttpPost]
        public IActionResult UpdateMedicinesStock([FromBody] List<MedicineStockUpdateModel> stockUpdates)
        {
            try
            {

                // Mostrar los datos recibidos en la consola
                Console.WriteLine("Datos recibidos para actualización de stock:");
                foreach (var update in stockUpdates)
                {
                    Console.WriteLine($"ID del Medicamento: {update.MedicineId}, Nuevo Stock: {update.NewStock}");
                }

                foreach (var update in stockUpdates)
                {
                    // Buscar el medicamento por ID
                    var medicine = db.Medicines.FirstOrDefault(m => m.Id == update.MedicineId);

                    if (medicine != null)
                    {
                        // Reemplazar el stock actual con el nuevo stock
                        medicine.Stock = update.NewStock;

                        // Actualizar la fecha de última modificación
                        medicine.LastUpdate = DateTime.Now;
                    }
                    else
                    {
                        return NotFound($"Medicamento con ID {update.MedicineId} no encontrado.");
                    }
                }

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                return Ok("Stocks actualizados exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el stock de los medicamentos: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<List<object>> NurseReport(int nurseId)
        {
            // Obtener la enfermera, su colegio y los medicamentos del botiquín
            var report = db.Nurses
                .Where(n => n.Id == nurseId)
                .Select(n => new
                {
                    NurseName = n.Email, // Puedes usar otro campo para el nombre si lo deseas
                    SchoolName = n.Assignments.Select(a => a.IdSchoolNavigation.Name).FirstOrDefault(),
                    Medicines = n.Assignments.SelectMany(a => a.IdSchoolNavigation.FirstAidKits)
                        .SelectMany(f => f.MedicineFirstAidKits)
                        .Select(m => new
                        {
                            MedicineName = m.IdMedicineNavigation.Name,
                            Stock = m.IdMedicineNavigation.Stock,
                            Provider = m.IdMedicineNavigation.Supplier.Name, // Asegúrate de que Supplier está relacionado
                            ExpirationDate = m.ExpirationDate
                        })
                })
                .FirstOrDefault();

            // Verificar si se encontró la enfermera
            if (report == null)
            {
                return NotFound();
            }

            // Retornar la vista con el reporte
            return View(report);
        }



    }

    public class MedicineStockUpdateModel
    {
        public short MedicineId { get; set; }
        public byte NewStock { get; set; }
    }
}
