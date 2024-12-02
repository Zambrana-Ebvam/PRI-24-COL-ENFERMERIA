using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Necesario para validaciones
using System.ComponentModel.DataAnnotations.Schema; // Para manejar atributos como DateOnly

namespace ProgramColegioEnfermeras.Models
{
    public partial class Nurse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden exceder los 100 caracteres.")]
        public string Names { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El primer apellido no puede exceder los 50 caracteres.")]
        public string FirstName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "El segundo apellido no puede exceder los 50 caracteres.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [RegularExpression("^(M|F)$", ErrorMessage = "El género debe ser M o F.")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de nacimiento debe ser una fecha válida.")]
        [CustomValidation(typeof(Nurse), nameof(ValidateBirthdate))]
        public DateOnly Birthdate { get; set; }

        [Required(ErrorMessage = "El CI es obligatorio.")]
        [StringLength(20, ErrorMessage = "El CI no puede exceder los 20 caracteres.")]
        public string Ci { get; set; } = null!;

        [Required(ErrorMessage = "El número de celular es obligatorio.")]
        [RegularExpression("^[67]\\d{7}$", ErrorMessage = "El número de celular debe comenzar con 6 o 7 y tener 8 dígitos.")]
        public string Cellphone { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150, ErrorMessage = "La dirección no puede exceder los 150 caracteres.")]
        public string Address { get; set; } = null!;

        [StringLength(10, ErrorMessage = "El código de sede no puede exceder los 10 caracteres.")]
        public string? CodeSedes { get; set; }

        [StringLength(100, ErrorMessage = "La especialidad no puede exceder los 100 caracteres.")]
        public string? Specialty { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Range(0, 1, ErrorMessage = "El estado debe ser 0 (Inactivo) o 1 (Activo).")]
        public byte State { get; set; } = 1; // Estado por defecto 1 (Activo)

        public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public virtual Userr? Userr { get; set; }

        // Validación personalizada para la fecha de nacimiento
        public static ValidationResult? ValidateBirthdate(DateOnly birthdate, ValidationContext context)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var minimumAge = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)); // Mayor de 18 años
            if (birthdate > today)
            {
                return new ValidationResult("La fecha de nacimiento no puede ser en el futuro.");
            }
            if (birthdate > minimumAge)
            {
                return new ValidationResult("El enfermero debe ser mayor de 18 años.");
            }
            return ValidationResult.Success;
        }
    }
}
