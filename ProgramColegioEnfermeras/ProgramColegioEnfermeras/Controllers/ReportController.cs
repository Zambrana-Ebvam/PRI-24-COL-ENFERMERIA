using Microsoft.AspNetCore.Mvc;
using ProgramColegioEnfermeras.Models;
using System.Drawing.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace ProgramColegioEnfermeras.Controllers
{
    public class ReportController : Controller
    {
        private readonly BdUnivalleEnfermerasContext _context;

        public ReportController(BdUnivalleEnfermerasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var annualData = GetAnnualReport();
            var monthlyData = GetMonthlyReport();
            var summaryByEstablishment = GetSummaryByEstablishment();

            ViewBag.AnnualReport = annualData;
            ViewBag.MonthlyReport = monthlyData;
            ViewBag.SummaryByEstablishment = summaryByEstablishment;

            return View();
        }


        private List<AnnualReportItem> GetAnnualReport()
        {
            var annualReport = _context.Kardices
                .GroupBy(k => new
                {
                    Year = k.RegistrationDate.Year,
                    EstablishmentId = k.IdStudentNavigation.IdEstablishment
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    EstablishmentId = g.Key.EstablishmentId,
                    Count = g.Select(k => k.IdStudent).Distinct().Count()
                })
                .ToList();

            var establishments = _context.Establishments.ToList();

            return annualReport.Select(r => new AnnualReportItem
            {
                Year = r.Year,
                EstablishmentName = establishments.FirstOrDefault(e => e.Id == r.EstablishmentId)?.Name ?? "Desconocido",
                Count = r.Count
            }).ToList();
        }
        // Método nuevo para mostrar estudiantes por establecimiento

        private List<MonthlyReportItem> GetMonthlyReport()
        {
            var monthlyReport = _context.Kardices
                .GroupBy(k => new
                {
                    Year = k.RegistrationDate.Year,
                    Month = k.RegistrationDate.Month,
                    EstablishmentId = k.IdStudentNavigation.IdEstablishment
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    EstablishmentId = g.Key.EstablishmentId,
                    Count = g.Select(k => k.IdStudent).Distinct().Count()
                })
                .ToList();

            var establishments = _context.Establishments.ToList();

            return monthlyReport.Select(r => new MonthlyReportItem
            {
                Year = r.Year,
                Month = r.Month,
                EstablishmentName = establishments.FirstOrDefault(e => e.Id == r.EstablishmentId)?.Name ?? "Desconocido",
                Count = r.Count
            }).ToList();
        }
        private List<EstablishmentSummaryItem> GetSummaryByEstablishment()
        {
            var summary = _context.Kardices
                .GroupBy(k => k.IdStudentNavigation.IdEstablishment)
                .Select(g => new EstablishmentSummaryItem
                {
                    EstablishmentId = g.Key,
                    EstablishmentName = g.First().IdStudentNavigation.IdEstablishmentNavigation.Name,
                    SickStudentsCount = g.Select(k => k.IdStudent).Distinct().Count()
                })
                .ToList();

            return summary;
        }
        [HttpPost]
        public IActionResult DownloadAnnualPdf()
        {
            var report = GetAnnualReport();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Reporte Anual de Estudiantes Enfermos por Establecimiento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                document.Add(new Paragraph(" "));

                var table = new PdfPTable(3);
                table.AddCell("Año");
                table.AddCell("Establecimiento");
                table.AddCell("Número de Estudiantes");

                foreach (var item in report)
                {
                    table.AddCell(item.Year.ToString());
                    table.AddCell(item.EstablishmentName);
                    table.AddCell(item.Count.ToString());
                }

                document.Add(table);
                document.Close();

                var file = memoryStream.ToArray();
                return File(file, "application/pdf", "Reporte_Anual_Estudiantes.pdf");
            }
        }

        [HttpPost]
        public IActionResult DownloadMonthlyPdf()
        {
            var report = GetMonthlyReport();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Reporte Mensual de Estudiantes Enfermos por Establecimiento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                document.Add(new Paragraph(" "));

                var table = new PdfPTable(4);
                table.AddCell("Año");
                table.AddCell("Mes");
                table.AddCell("Establecimiento");
                table.AddCell("Número de Estudiantes");

                foreach (var item in report)
                {
                    table.AddCell(item.Year.ToString());
                    table.AddCell(item.Month.ToString("00"));
                    table.AddCell(item.EstablishmentName);
                    table.AddCell(item.Count.ToString());
                }

                document.Add(table);
                document.Close();

                var file = memoryStream.ToArray();
                return File(file, "application/pdf", "Reporte_Mensual_Estudiantes.pdf");
            }
        }

        [HttpPost]
        public IActionResult DownloadSummaryPdf()
        {
            var summary = GetSummaryByEstablishment();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Resumen de Estudiantes Enfermos por Establecimiento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                document.Add(new Paragraph(" "));

                var table = new PdfPTable(2);
                table.AddCell("Establecimiento");
                table.AddCell("Número de Estudiantes Enfermos");

                foreach (var item in summary)
                {
                    table.AddCell(item.EstablishmentName);
                    table.AddCell(item.SickStudentsCount.ToString());
                }

                document.Add(table);
                document.Close();

                var file = memoryStream.ToArray();
                return File(file, "application/pdf", "Resumen_Estudiantes_Enfermos_Por_Establecimiento.pdf");
            }
        }

        [HttpPost]
        public IActionResult DownloadStudentsPerEstablishmentPdf(byte establishmentId)
        {
            var students = _context.Kardices
                .Where(k => k.IdStudentNavigation.IdEstablishment == establishmentId)
                .Select(k => new StudentItem
                {
                    Id = k.IdStudent,
                    Names = k.IdStudentNavigation.Names,
                    FirstName = k.IdStudentNavigation.FirstName,
                    MiddleName = k.IdStudentNavigation.MiddleName,
                    Gender = k.IdStudentNavigation.Gender,
                    Birthdate = k.IdStudentNavigation.Birthdate,
                    Code = k.IdStudentNavigation.Code,
                    Tutor = k.IdStudentNavigation.Tutor,
                    TutorCellphone = k.IdStudentNavigation.TutorCellphone,
                    BloodType = k.IdStudentNavigation.BloodType,
                    Allergy = k.IdStudentNavigation.Allergy,
                    RegistrationDate = k.RegistrationDate
                })
                .Distinct()
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.Names)
                .ToList();

            var establishmentName = _context.Establishments
                .Where(e => e.Id == establishmentId)
                .Select(e => e.Name)
                .FirstOrDefault();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph($"Estudiantes Atendidos en {establishmentName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                document.Add(new Paragraph(" "));

                var table = new PdfPTable(11);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3f, 4f, 4f, 2f, 3f, 3f, 4f, 4f, 3f, 4f, 4f });

                table.AddCell("ID");
                table.AddCell("Nombres");
                table.AddCell("Apellido");
                table.AddCell("Género");
                table.AddCell("Fecha Nac.");
                table.AddCell("Código");
                table.AddCell("Tutor");
                table.AddCell("Cel. Tutor");
                table.AddCell("Tipo Sangre");
                table.AddCell("Alergias");
                table.AddCell("Fecha Atención");

                foreach (var student in students)
                {
                    table.AddCell(student.Id.ToString());
                    table.AddCell(student.Names);
                    table.AddCell(student.FirstName);
                    table.AddCell(student.Gender);
                    table.AddCell(student.Birthdate.ToString("dd/MM/yyyy"));
                    table.AddCell(student.Code ?? "");
                    table.AddCell(student.Tutor);
                    table.AddCell(student.TutorCellphone);
                    table.AddCell(student.BloodType ?? "");
                    table.AddCell(student.Allergy);
                    table.AddCell(student.RegistrationDate.ToString("dd/MM/yyyy HH:mm"));
                }

                document.Add(table);
                document.Close();

                var file = memoryStream.ToArray();
                return File(file, "application/pdf", $"Estudiantes_Atendidos_{establishmentName}.pdf");
            }
        }
        public class AnnualReportItem
        {
            public int Year { get; set; }
            public string EstablishmentName { get; set; }
            public int Count { get; set; }
        }

        public class MonthlyReportItem
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public string EstablishmentName { get; set; }
            public int Count { get; set; }
        }

        public class EstablishmentSummaryItem
        {
            public int EstablishmentId { get; set; }
            public string EstablishmentName { get; set; }
            public int SickStudentsCount { get; set; }
        }


        public class StudentItem
        {
            public int Id { get; set; }
            public string Names { get; set; }
            public string FirstName { get; set; }
            public string? MiddleName { get; set; }
            public string Gender { get; set; }
            public DateOnly Birthdate { get; set; }
            public string? Code { get; set; }
            public string Tutor { get; set; }
            public string TutorCellphone { get; set; }
            public string? BloodType { get; set; }
            public string Allergy { get; set; }
            public DateTime RegistrationDate { get; set; }
        }
    }
}
