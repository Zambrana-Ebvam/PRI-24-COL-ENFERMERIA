using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace ProgramColegioEnfermeras.Models;

public class Userr
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 30 caracteres.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;

    [Required]
    public DateTime RegistrationDate { get; set; }

    public DateTime? UpdateDate { get; set; }
   
    public byte? State { get; set; }

    public virtual Nurse? IdNavigation { get; set; }
}


// Helper para hashing
public static class HashHelper
{
    // Método para generar el hash y devolverlo como string en base64
    public static string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Cifrar la cadena
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convertir el hash a una cadena de base64
            return Convert.ToBase64String(bytes);
        }
    }

    // Método para verificar si la contraseña ingresada es igual al hash almacenado
    public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
        // Cifrar la contraseña ingresada
        var enteredPasswordHash = ComputeSha256Hash(enteredPassword);

        // Comparar el hash cifrado con el almacenado en la base de datos
        return enteredPasswordHash == storedPasswordHash;
    }
}
