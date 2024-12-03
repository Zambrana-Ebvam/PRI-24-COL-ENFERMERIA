
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Models;
using ProyectoFnalIntegrado.Services;

namespace ProyectoFnalIntegrado.Controllers
{
    public class First_Aid_KitController : Controller
    {
        private readonly DaContext db;
        private readonly DatabaseHelper dbHelper;
        private static readonly object _lock = new object();
        private static int _lastGeneratedId;

        public static int LastGeneratedId
        {
            get
            {
                lock (_lock)
                {
                    return _lastGeneratedId;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _lastGeneratedId = value;
                }
            }
        }
        public First_Aid_KitController(DaContext context)
        {
            db = context;
            dbHelper = new DatabaseHelper(context); // Instanciar DatabaseHelper
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
                var suppliers = db.Establishments
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
            // GET /First_Aid_Kit/GetNursesBySchool?schoolId=1
            try
            {
                var nurses = db.Assignments
                    .Where(a => a.IdSchool == schoolId && a.Status == 1)
                    .Join(db.Nurses,
                          a => a.IdNurse,
                          n => n.Id,
                          (a, n) => new
                          {
                              NurseId = n.Id,
                              FullName = $"{n.Names} {n.MiddleName}" // Asumiendo que estas columnas existen en Nurses
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

     



        public async Task<IActionResult> CreateFirstAidKitAsync([FromBody] FirstAidKit firstAidKit)
        {
          
            Console.WriteLine($"Recibido: ResponsibleId={firstAidKit.ReponsibleId}");
            Console.WriteLine("Creando FirstAidKit...");
            Console.WriteLine($"FirstAidKit: Nombre={firstAidKit.Name}, Descripción={firstAidKit.Description}, SchoolId={firstAidKit.SchoolId}");

         
            db.Set<FirstAidKit>().Add(firstAidKit);
            await db.SaveChangesAsync();

          
            var establishment = await db.Set<Establishment>()
                .FirstOrDefaultAsync(e => e.Id == firstAidKit.SchoolId);

            if (establishment != null)
            {
                Console.WriteLine($"Establecimiento encontrado: Nombre={establishment.Name}, Id={establishment.Id}");
            }
            else
            {
                Console.WriteLine("No se encontró el establecimiento.");
            }

            
            var result = new
            {
                firstAidKitId = firstAidKit.Id,
                establishment = new
                {
                    id = establishment?.Id,
                    name = establishment?.Name
                }
            };
            Console.WriteLine($"Datos retornados: FirstAidKitId={result.firstAidKitId}, EstablishmentId={result.establishment.id}, NameEstablishment={result.establishment.name}");

            return Ok(result);
        }



        public class FirstAidKitRequestDTO
        {
            public int FirstAidKitId { get; set; }
            public List<MedicineFirstAidKitDTO> Medicines { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicinesToFirstAidKitAsync([FromBody] FirstAidKitRequestDTO request)
        {
            int firstAidKitId = request.FirstAidKitId;
            List<MedicineFirstAidKitDTO> medicines = request.Medicines;

            Console.WriteLine($"ID del botiquín recibido: {firstAidKitId}");

            foreach (var medicine in medicines)
            {
                // Verificar si el medicamento existe en la base de datos
                var existsInDb = await db.Medicines.AnyAsync(m => m.Id == medicine.IdMedicine);
                if (!existsInDb)
                {
                    Console.WriteLine($"Alerta: El medicamento con Id {medicine.IdMedicine} no existe en la base de datos y no será añadido al botiquín.");
                    continue;
                }

                // Verificar si el medicamento ya está asociado con el botiquín
                var existingMedicine = await db.MedicineFirstAidKits
                    .FirstOrDefaultAsync(m => m.IdAid == firstAidKitId && m.IdMedicine == medicine.IdMedicine);

                if (existingMedicine != null)
                {
                    // Actualizar la cantidad del medicamento
                    existingMedicine.Quantity += (byte)medicine.Quantity;
                    existingMedicine.LastUpdated = DateTime.Now;
                    Console.WriteLine($"Cantidad actualizada para el medicamento Id {medicine.IdMedicine}. Nueva cantidad: {existingMedicine.Quantity}");
                }
                else
                {
                    // Crear un nuevo registro de medicamento en el botiquín
                    var newMedicine = new MedicineFirstAidKit
                    {
                        IdAid = (byte)firstAidKitId,  // Usar int si es adecuado
                        IdMedicine = medicine.IdMedicine,
                        Quantity = medicine.Quantity,
                        ExpirationDate = medicine.ExpirationDate ?? DateTime.Now.AddYears(1),
                        LastUpdated = DateTime.Now
                    };
                    db.MedicineFirstAidKits.Add(newMedicine);
                    Console.WriteLine($"Nuevo medicamento agregado: {medicine.IdMedicine} con cantidad {medicine.Quantity}");
                }
            }

            await db.SaveChangesAsync(); // Guardar los cambios en la base de datos
  
            return RedirectToAction("FillStock", "Supplier");
        }






        public class MedicineFirstAidKitDTO
        {
            public short IdMedicine { get; set; }
            public int Quantity { get; set; }
            public DateTime? ExpirationDate { get; set; } // Este campo es opcional
        }
    }
}
