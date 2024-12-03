
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;

namespace ProyectoFnalIntegrado.Controllers
{
    public class MedicineController : Controller
    {
        private readonly DaContext db;
        public MedicineController()
        {
            db = new DaContext();
        }
        public ActionResult CreateMedicine()
        {
            return View();
        }
        public ActionResult MedicationList()
        {
            return View();
        }

        // POST: Insertar un nuevo proveedor
        [HttpPost]
        public IActionResult InsertMedicine([FromBody] Medicine medicine)
        {
            try
            {

                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {

                    return Unauthorized("Usuario no autenticado.");
                }

                var newMedicine = new Medicine
                {
                    Name = medicine.Name,
                    Stock = medicine.Stock,
                    Description = medicine.Description,
                    ExpirationDate = medicine.ExpirationDate,
                    RegisterDate = DateTime.Now,
                    UserId = (int)userId,
                    Status = 1,
                    CategoryId = medicine.CategoryId,
                    SupplierId = medicine.SupplierId
                };

                db.Medicines.Add(newMedicine);
                db.SaveChanges();

                return Ok("Medicamento creado exitosamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al crear el medicamento: {ex.Message} - Detalles: {ex.InnerException?.Message}";
                return BadRequest(errorMessage);
            }
        }

        // Actualizar medicamento

        [HttpPost]
        public IActionResult UpdateMedicine([FromBody] Medicine medicine)
        {
            try
            {
                // Verificar si el ID es nulo
                if (medicine.Id == 0) // Asumiendo que Id es int y comienza desde 1
                {
                    return BadRequest("El ID del medicamento no puede ser nulo.");
                }

                // Buscar el medicamento en la base de datos por ID
                var medicineup = db.Medicines.Find(medicine.Id);
                if (medicineup == null)
                {
                    return NotFound("Medicamento no encontrado.");
                }

                // Actualizar las propiedades de la entidad cargada
                medicineup.Name = medicine.Name;
                medicineup.Description = medicine.Description;

                // Asignar ExpirationDate como DateOnly
                medicineup.ExpirationDate = medicine.ExpirationDate; // Asegúrate de que este valor sea parseable
                medicineup.Status = 1;
                medicineup.LastUpdate = DateTime.Now;
                medicineup.CategoryId = medicine.CategoryId;
                medicineup.SupplierId = medicine.SupplierId;

                // Guardar cambios
                db.SaveChanges();

                return Ok("Medicamento actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el medicamento: {ex.Message}");
            }
        }



        // GET: Recuperar todos los proveedore+s activos
        [HttpGet]
        public ActionResult<List<object>> GetActiveSuppliers()
        {
            try
            {
                var suppliers = db.Suppliers
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

        // GET: Recuperar todos las categorias activas
        [HttpGet]
        public ActionResult<List<object>> GetActiveCategories()
        {
            try
            {
                var categories = db.Categories
                                  .Select(s => new
                                  {
                                      s.Id,
                                      s.Name
                                  })
                                  .ToList();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar las categorias: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetActiveMedicines()
        {
            try
            {
              
                var activeMedicines = await db.Medicines
                    .Where(m => m.Status == 1) 
                    .OrderByDescending(m => m.Id) 
                    .Select(m => new
                    {
                        MedicineId = m.Id, 
                        MedicineName = m.Name, 
                        MedicineDescription = m.Description, 
                        CategoryId = m.CategoryId, 
                        CategoryName = m.Category.Name, 
                        SupplierId = m.SupplierId, 
                        SupplierName = m.Supplier.Name, 
                        ExpirationDate = m.ExpirationDate,
                        RegisterDate = m.RegisterDate.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return Ok(activeMedicines);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al recuperar los medicamentos activos: {ex.Message}");
            }
        }


        //Eliminar o desactivar un proveedor
        [HttpPost]
        public IActionResult DeleteMedicine(short id)
        {
            try
            {
                var supplier = db.Medicines.Find(id);
                if (supplier == null)
                {
                    return NotFound("Medicamento no encontrado.");
                }

                supplier.Status = 0;
                supplier.LastUpdate = DateTime.Now;

                db.Medicines.Update(supplier);
                db.SaveChanges();

                return Ok("Medicamento desactivado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al desactivar el medicamento: {ex.Message}");
            }
        }


    }
}
