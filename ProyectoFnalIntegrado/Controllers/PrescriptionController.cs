using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProyectoFnalIntegrado.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly DaContext _context;

        public PrescriptionController(DaContext context)
        {
            //_context = context;

            var optionsBuilder = new DbContextOptionsBuilder<DaContext>();
            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
            _context = new DaContext(optionsBuilder.Options);
        }

        public IActionResult CRUD()
        {
            int? idEstablishment = HttpContext.Session.GetInt32("IdEstablisment");

            if (idEstablishment == null)
            {
                // Si no hay ID de establecimiento en la sesión, redirigir o manejar el error
                return RedirectToAction("Error");
            }

            // Crear las listas para almacenar los resultados de los procedimientos
            var medicines = new List<object>();
            var nursesInEstablishment = new List<object>();
            var studentsInEstablishment = new List<object>();

            // Cadena de conexión a la base de datos
            string connectionString = AppSettings.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Llamar al procedimiento almacenado para obtener los medicamentos
                using (SqlCommand command = new SqlCommand("GetMedicinesByEstablishment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SchoolId", idEstablishment);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            medicines.Add(new
                            {
                                Id = reader["MedicineId"],
                                Name = reader["MedicineName"]
                            });
                        }
                    }
                }

                // Llamar al procedimiento almacenado para obtener las enfermeras
                using (SqlCommand command = new SqlCommand("GetNursesByEstablishment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EstablishmentId", idEstablishment);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nursesInEstablishment.Add(new
                            {
                                Id = reader["Id"],
                                Names = reader["Names"]
                            });
                        }
                    }
                }

                // Llamar al procedimiento almacenado para obtener los estudiantes
                using (SqlCommand command = new SqlCommand("GetStudentsByEstablishment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EstablishmentId", idEstablishment);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentsInEstablishment.Add(new
                            {
                                Id = reader["Id"],
                                FirstName = reader["FirstName"]
                            });
                        }
                    }
                }
            }

            // Pasar los medicamentos al ViewBag como una lista SelectList
            ViewBag.MedicineId = new SelectList(medicines, "Id", "Name");

            // Pasar los estudiantes y enfermeras al ViewBag
            ViewBag.StudentId = new SelectList(studentsInEstablishment, "Id", "FirstName");
            ViewBag.NurseId = new SelectList(nursesInEstablishment, "Id", "Names");

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetAllPrescriptions()
        {
            var prescriptions = await _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.Student)
                .Include(p => p.Nurse)
                .Where(p => p.Status == 1)
                .Select(p => new {
                    id = p.Id,
                    dosage = p.Dosage,
                    frecuency = p.Frecuency,
                    instructions = p.Instructions,
                    medicineName = p.Medicine.Name,
                    studentName = p.Student.FirstName,
                    nurseName = p.Nurse.FirstName
                }).ToListAsync();

            return Json(prescriptions);
        }

        [HttpPost]
        public IActionResult Create(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                prescription.Status = 1;
                _context.Prescriptions.Add(prescription);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Datos no válidos." });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var prescription = _context.Prescriptions.Find(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return Json(prescription);
        }

        [HttpPost]
        public IActionResult Edit(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                var existingPrescription = _context.Prescriptions.Find(prescription.Id);
                if (existingPrescription == null)
                {
                    return Json(new { success = false, message = "Prescripción no encontrada." });
                }

                existingPrescription.Dosage = prescription.Dosage;
                existingPrescription.Frecuency = prescription.Frecuency;
                existingPrescription.Instructions = prescription.Instructions;
                existingPrescription.MedicineId = prescription.MedicineId;
                existingPrescription.StudentId = prescription.StudentId;
                existingPrescription.NurseId = prescription.NurseId;
                existingPrescription.StarDate = prescription.StarDate;
                existingPrescription.EndDate = prescription.EndDate;

                _context.SaveChanges();
                return Json(new { success = true, message = "Prescripción actualizada correctamente." });
            }

            return Json(new { success = false, message = "Datos no válidos." });
        }

        [HttpPost]
        public IActionResult DeleteLogical(int id)
        {
            var prescription = _context.Prescriptions.Find(id);
            if (prescription == null)
            {
                return NotFound();
            }

            prescription.Status = 0;
            _context.Prescriptions.Update(prescription);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
