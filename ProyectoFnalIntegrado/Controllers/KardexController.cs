using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace ProyectoFnalIntegrado.Controllers
{
    public class KardexController : Controller
    {
        private readonly DaContext _context;

        public KardexController(DaContext context)
        {
            //_context = context;
            var optionsBuilder = new DbContextOptionsBuilder<DaContext>();
            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
            _context = new DaContext(optionsBuilder.Options);
        }

        public IActionResult CRUD()
        {
            try
            {
                // Obtener el ID del establecimiento desde la sesión
                int? establishmentId = HttpContext.Session.GetInt32("IdEstablisment");

                if (establishmentId == null)
                {
                    // Manejar el caso en que no haya un establecimiento en la sesión
                    return BadRequest("Establishment ID not found.");
                }

                // Crear la lista para almacenar los resultados
                var nursesInEstablishment = new List<object>();
                var studentsInEstablishment = new List<object>();

                // Establecer la cadena de conexión
                string connectionString = AppSettings.ConnectionString;

                // Establecer la conexión con la base de datos (usando ADO.NET)
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Llamar al procedimiento almacenado para obtener las enfermeras
                    using (SqlCommand command = new SqlCommand("GetNursesByEstablishment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EstablishmentId", establishmentId);

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
                        command.Parameters.AddWithValue("@EstablishmentId", establishmentId);

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

                // Verificar si se encontraron datos
                if (!nursesInEstablishment.Any() && !studentsInEstablishment.Any())
                {
                    return NotFound("No se encontraron enfermeras ni estudiantes para este establecimiento.");
                }

                // Pasar las listas a las vistas usando ViewBag
                ViewBag.NurseId = new SelectList(nursesInEstablishment, "Id", "Names");
                //ViewBag.StudentId = new SelectList(studentsInEstablishment, "Id", "FirstName");
                ViewBag.StudentId = new SelectList(studentsInEstablishment, "Id", "FirstName");

                return View();
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra durante la ejecución
                return BadRequest($"Error al recuperar los datos: {ex.Message}");
            }


       
        }



    [HttpGet]
        public async Task<JsonResult> GetAllKardexes()
        {
            var kardexes = await _context.Kardices
                .Include(k => k.IdStudentNavigation)
                .Include(k => k.IdNurseNavigation)
                .Where(k => k.State == 1)
                .Select(k => new {
                    id = k.Id,
                    height = k.Height,
                    weight = k.Weight,
                    oxygenLevel = k.OxygenLevel,
                    description = k.Description,
                    temperature = k.Temperature,
                    respiratoryRate = k.RespiratoryRate,
                    bloodPressure = k.BloodPressure,
                    derivation = k.Derivation,
                    studentName = k.IdStudentNavigation.FirstName,
                    nurseName = k.IdNurseNavigation.Names
                }).ToListAsync();

            return Json(kardexes);
        }

        [HttpPost]
        public IActionResult Create(Kardex kardex)
        {
            if (ModelState.IsValid)
            {
                kardex.State = 1;
                _context.Kardices.Add(kardex);
                _context.SaveChanges();
                return Json(new { success = true, message = "Kardex creado exitosamente." });
            }
            return Json(new { success = false, message = "Datos no válidos." });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kardex = _context.Kardices.Find(id);
            if (kardex == null)
            {
                return Json(new { success = false, message = "Kardex no encontrado." });
            }
            return Json(kardex);
        }

        [HttpPost]
        public IActionResult Edit(Kardex kardex)
        {
            if (ModelState.IsValid)
            {
                var existingKardex = _context.Kardices.Find(kardex.Id);
                if (existingKardex == null)
                {
                    return Json(new { success = false, message = "Kardex no encontrado." });
                }

                // Actualizar propiedades del Kardex existente
                existingKardex.Height = kardex.Height;
                existingKardex.Weight = kardex.Weight;
                existingKardex.OxygenLevel = kardex.OxygenLevel;
                existingKardex.Description = kardex.Description;
                existingKardex.Temperature = kardex.Temperature;
                existingKardex.RespiratoryRate = kardex.RespiratoryRate;
                existingKardex.BloodPressure = kardex.BloodPressure;
                existingKardex.Derivation = kardex.Derivation;
                existingKardex.IdStudent = kardex.IdStudent;
                existingKardex.IdNurse = kardex.IdNurse;

                _context.SaveChanges();
                return Json(new { success = true, message = "Kardex actualizado correctamente." });
            }
            return Json(new { success = false, message = "Datos no válidos." });
        }

        [HttpPost]
        public IActionResult DeleteLogical(int id)
        {
            var kardex = _context.Kardices.Find(id);
            if (kardex == null)
            {
                return Json(new { success = false, message = "Kardex no encontrado." });
            }

            kardex.State = 0;
            _context.Kardices.Update(kardex);
            _context.SaveChanges();

            return Json(new { success = true, message = "Kardex eliminado correctamente." });
        }

    }
}
