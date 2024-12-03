using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProyectoFnalIntegrado.Services
{
    public class EmailService
    {
        // Configuración del servidor SMTP
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "axeroth123@gmail.com"; // Correo del remitente
        private readonly string _smtpPass = "qitzbvbxkplyfzsh"; // Contraseña de aplicación

        // Método para enviar correos electrónicos genéricos
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                smtpClient.EnableSsl = true; // Usar conexión segura

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser, "Equipo Univalle"), // Nombre del remitente
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Permitir HTML en el correo
                };

                mailMessage.To.Add(toEmail); // Agregar destinatario

                try
                {
                    await smtpClient.SendMailAsync(mailMessage); // Enviar correo
                }
                catch (SmtpException smtpEx)
                {
                    // Manejo de errores específicos del servidor SMTP
                    throw new InvalidOperationException("Error en el servidor SMTP al enviar el correo", smtpEx);
                }
                catch (Exception ex)
                {
                    // Manejo de errores generales
                    throw new InvalidOperationException("Error inesperado al enviar el correo", ex);
                }
            }
        }

        // Método específico para enviar credenciales
        public async Task SendCredentialsAsync(string toEmail, string username, string password)
        {
            string subject = "Credenciales de Acceso - Enfermería Univalle";
            string body = $@"
                <h2 style='color: #4CAF50;'>Bienvenido/a a Enfermería Univalle</h2>
                <p>Se le han asignado las siguientes credenciales de acceso:</p>
                <table style='border: 1px solid #ddd; border-collapse: collapse; width: 50%;'>
                    <tr style='background-color: #f2f2f2;'>
                        <th style='padding: 8px; text-align: left; border: 1px solid #ddd;'>Usuario</th>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{username}</td>
                    </tr>
                    <tr>
                        <th style='padding: 8px; text-align: left; border: 1px solid #ddd;'>Contraseña</th>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{password}</td>
                    </tr>
                </table>
                <p>Por favor, cambie su contraseña al iniciar sesión por primera vez.</p>
                <p>Atentamente,</p>
                <p><strong>Equipo de Enfermería Univalle</strong></p>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
