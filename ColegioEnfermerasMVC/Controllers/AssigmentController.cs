using ColegioEnfermerasMVC.Data;
using ColegioEnfermerasMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace ColegioEnfermerasMVC.Controllers
{
    public class AssigmentController : Controller
    {
        private readonly DbCollegeContext db;

        public AssigmentController()
        {
            db = new DbCollegeContext();
        }
        public IActionResult AssigmentNurse()
        {
            return View();
        }

        public IActionResult AssigmentNurseAndSchool()
        {
            return View();
        }

        [HttpGet]
        //lista las asignaciones actuales
        public ActionResult<string> GetNurseSchoolAssignments()
        {
            try
            {
                var assignments = db.GetNurseSchoolAssignments();
                string assignmentsJson = JsonConvert.SerializeObject(assignments);
                return assignmentsJson;
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        //lista las enfermeras activas
        [HttpGet]
        public ActionResult<string> GetAllNurse()
        {
            try
            {
                var assignments = db.GetAllNurses();
                string assignmentsJson = JsonConvert.SerializeObject(assignments);
                return assignmentsJson;
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
         
        }
        //lista las escuelas activas
        [HttpGet]
        public ActionResult<string> GetAllSchools()
        {

            try
            {
                var assignments = db.GetAllSchools();
                string assignmentsJson = JsonConvert.SerializeObject(assignments);
                return assignmentsJson;
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        //Lista las asignaciones
        [HttpPost]
        public IActionResult AssignNurseToSchool([FromBody] Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest("Datos de asignación inválidos.");
            }
            try
            {
                using (var context = new DbCollegeContext())
                {
                    assignment.Status = 1; 
                    context.Assignments.Add(assignment);
                    context.SaveChanges();
                }

                return Ok("Asignación exitosa.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // Reasignar enfermero a una escuela
        [HttpPost]
        public IActionResult ReassignNurseToSchool([FromBody] RequestData requestData)
        {
          
            var assignment = requestData.Assignment;
            int currentSchoolId = requestData.CurrentSchoolId; 
            try
            {
               
                bool result = db.ReassignNurseToSchool(assignment, currentSchoolId); 
                if (result)
                {
                    return Ok("Asignación exitosa.");
                }
                else
                {
                    return BadRequest("Error al reasignar el enfermero.");  
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);  
            }
        }

        //busqueda de escuelas por zonas
        [HttpGet]
        public ActionResult<string> GetSchoolsByZone(string zona = "Todas")
        {
            try
            {
                var schools = db.GetSchoolsByZone(zona);
                string schoolsJson = JsonConvert.SerializeObject(schools);
                return schoolsJson;
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        //Historico de del enfermero
        [HttpGet]
        public ActionResult<string> GetNurseHistoryAssignments(int idNurse)
        {
            try
            {
                var schools = db.GetNurseHistoryAssignments(idNurse);
                string schoolsJson = JsonConvert.SerializeObject(schools);
                return schoolsJson;
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // Clase para deserializar los datos recibidos
        public class RequestData
        {
            public Assignment? Assignment { get; set; }
            public int CurrentSchoolId { get; set; }
        }
    }
}
