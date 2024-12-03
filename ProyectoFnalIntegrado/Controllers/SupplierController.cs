
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using System;
using System.Data;
using System.Linq;

namespace ProyectoFnalIntegrado.Controllers
{
    public class SupplierController : Controller
    {
        private readonly DaContext db;

        public SupplierController()
        {
            //db = new DaContext();
            var optionsBuilder = new DbContextOptionsBuilder<DaContext>();
            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
            db = new DaContext(optionsBuilder.Options);
        }

        public ActionResult CreateSupplier()
        {
            return View();
        }

        public ActionResult FillStock()
        {
            return View();
        }

      
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
                                      RegisterDate = s.RegistraterDate.ToString("yyyy-MM-dd"),
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

        [HttpGet]
        public ActionResult<List<object>> GetMedicinesByFirstAidKit()
        {
            try
            {
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
                if (idEstablishment == null)
                    return BadRequest("No se encontró el ID del establecimiento en la sesión.");

                var medicines = db.FirstAidKits
                    .Where(fak => fak.Status == 1 && fak.SchoolId == idEstablishment && fak.MedicineFirstAidKits.Any())
                    .SelectMany(fak => fak.MedicineFirstAidKits)
                    .Select(mfak => new
                    {
                        mfak.IdMedicine1!.Id,          // ID del medicamento
                        mfak.IdMedicine1.Name,        // Nombre del medicamento
                        mfak.Quantity,                // Cantidad en el botiquín
                        mfak.IdMedicine1.Description, // Descripción del medicamento
                        mfak.IdMedicine1.ExpirationDate,
                        mfak.IdMedicine1.Stock,
                        mfak.IdMedicine1.CategoryId,
                        mfak.IdMedicine1.SupplierId
                    })
                    .Distinct()
                    .OrderBy(med => med.Id)
                    .ToList();

                return medicines.Any()
                    ? Ok(medicines)
                    : NotFound($"No se encontraron medicamentos asociados para el establecimiento con ID {idEstablishment}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los medicamentos: {ex.Message}");
            }
        }

      

        [HttpGet]
        public ActionResult<List<object>> GetSuppliersByEstablishment()
        {
            try
            {
                // Obtener el ID del establecimiento desde la sesión
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
                if (idEstablishment == null)
                {
                    return BadRequest("Establecimiento no encontrado en la sesión.");
                }

                // Crear la lista para almacenar los resultados
                var suppliers = new List<object>();

                // Establecer la conexión con la base de datos usando la cadena de conexión directa
                using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
                {
                    connection.Open();

                    // Crear el comando SQL para ejecutar el procedimiento almacenado
                    using (SqlCommand command = new SqlCommand("GetSuppliersByEstablishment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar el parámetro para el ID del establecimiento
                        command.Parameters.AddWithValue("@IdEstablishment", idEstablishment);

                        // Ejecutar la consulta y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Leer los datos y agregarlos a la lista
                                suppliers.Add(new
                                {
                                    Id = reader["Id"],
                                    Name = reader["Name"],
                                    Adress = reader["Adress"],
                                    Phone = reader["Phone"],
                                    RegisterDate = reader["RegisterDate"],
                                    LastUpdate = reader["LastUpdate"],
                                    UserId = reader["UserId"],
                                    Status = reader["Status"]
                                });
                            }
                        }
                    }
                }

                // Verificar si se encontraron proveedores
                return suppliers.Any()
                    ? Ok(suppliers)
                    : NotFound($"No se encontraron proveedores asociados para el establecimiento con ID {idEstablishment}.");
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver mensaje de excepción
                return BadRequest($"Error al recuperar los proveedores: {ex.Message}");
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
                    RegistraterDate = DateTime.Now,
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

                // Recuperar el ID del establecimiento desde la sesión
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
                if (idEstablishment == null)
                    return BadRequest("No se encontró el ID del establecimiento en la sesión.");

                // Crear la lista para almacenar los resultados
                var medicines = new List<object>();

                // Establecer la conexión con la base de datos (ajusta tu cadena de conexión)
                using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
                {
                    connection.Open();

                    // Crear el comando SQL para ejecutar el procedimiento almacenado
                    using (SqlCommand command = new SqlCommand("GetMedicinesBySupplierAndSchool", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros para el procedimiento almacenado
                        command.Parameters.AddWithValue("@SchoolId", idEstablishment);
                        command.Parameters.AddWithValue("@SupplierId", id);

                        // Ejecutar la consulta y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Leer los datos y agregarlos a la lista
                                medicines.Add(new
                                {
                                    Id = reader["Id"],
                                    Name = reader["Name"],
                                    Quantity = reader["Quantity"]
                                });
                            }
                        }
                    }
                }

                Console.WriteLine($"Número de medicamentos encontrados: {medicines.Count}");

                // Verificar si se encontraron medicamentos
                if (!medicines.Any())
                {
                    return NotFound($"No se encontraron medicamentos asociados al proveedor con ID {id} para este establecimiento.");
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

                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
              
                var suppliers = db.FirstAidKits
                    .Where(fak => fak.Status == 1 && fak.SchoolId == idEstablishment && fak.MedicineFirstAidKits.Any())
                    .SelectMany(fak => fak.MedicineFirstAidKits)
                    .Select(mfak => mfak.IdMedicine1)
                    .Where(med => med != null)
                    .Select(med => med!.Supplier)
                    .Distinct()
                    .Select(supplier => new
                    {
                        supplier!.Id,
                        supplier.Name
                      
                    })
                    .OrderByDescending(s => s.Id)
                    .ToList();

                return suppliers.Any()
                    ? Ok(suppliers)
                    : NotFound($"No se encontraron proveedores asociados para el establecimiento con ID {idEstablishment}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los proveedores: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult UpdateMedicinesStock([FromBody] List<MedicineStockUpdateModel> stockUpdates)
        {
            try
            {
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
                if (idEstablishment == null)
                {
                    return BadRequest("No se encontró el ID del establecimiento en la sesión.");
                }

                // Convertir stockUpdates a JSON
                var stockUpdatesJson = System.Text.Json.JsonSerializer.Serialize(stockUpdates);

                // Ejecutar el procedimiento almacenado
                var parameters = new[]
                {
            new SqlParameter("@IdEstablishment", idEstablishment.Value),
            new SqlParameter("@StockUpdates", stockUpdatesJson)
        };

                db.Database.ExecuteSqlRaw("EXEC UpdateMedicinesStock @IdEstablishment, @StockUpdates", parameters);

                return Ok("Stocks actualizados exitosamente.");
            }
            catch (SqlException ex)
            {
                return BadRequest($"Error al actualizar el stock de los medicamentos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error general: {ex.Message}");
            }
        }



        [HttpGet]
        public ActionResult<object> EstablishmentReport()
        {
            try
            {
                // Obtener el ID del establecimiento desde la sesión
                int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");
                if (idEstablishment == null)
                {
                    return Unauthorized("Usuario no autenticado.");
                }

                // Generar el reporte
                var report = db.FirstAidKits
                    .Where(fak => fak.SchoolId == idEstablishment && fak.Status == 1)
                    .Select(fak => new
                    {
                        SchoolName = fak.School.Name, // Nombre del establecimiento
                        Medicines = fak.MedicineFirstAidKits
                            .Select(mfak => new
                            {
                                MedicineName = mfak.IdMedicine1.Name,  // Nombre del medicamento
                                QuantityAdded = mfak.Quantity,         // Cantidad en el botiquín
                                Provider = mfak.IdMedicine1.Supplier!.Name, // Nombre del proveedor
                                ExpirationDate = mfak.ExpirationDate    // Fecha de expiración
                            })
                            .OrderBy(m => m.MedicineName) // Ordenar por nombre de medicina
                            .ToList()
                    })
                    .FirstOrDefault();

                // Verificar si se encontró el establecimiento
                if (report == null)
                {
                    return NotFound($"No se encontraron datos para el establecimiento con ID {idEstablishment}.");
                }

                // Retornar el reporte
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar el reporte: {ex.Message}");
            }
        }




    }

    public class MedicineStockUpdateModel
    {
        public short MedicineId { get; set; }
        public int NewStock { get; set; }
    }
}
