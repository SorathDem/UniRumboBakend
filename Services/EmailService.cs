// Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using UniRumboBakend.Services.Interfaces;

namespace UniRumboBakend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var smtpHost = _config["Smtp:Host"];           // ej: smtp.gmail.com
            var smtpPort = int.Parse(_config["Smtp:Port"]!); // ej: 587
            var smtpUser = _config["Smtp:Username"];
            var smtpPass = _config["Smtp:Password"];
            var fromEmail = _config["Smtp:FromEmail"];
            var fromName = _config["Smtp:FromName"];

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "UniRumbo - Restablecer tu contraseña",
                IsBodyHtml = true,
                Body = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Restablecer contraseña</title>
    <style>
        body {{ font-family: Arial, sans-serif; background:#f6f9fc; padding:20px; }}
        .container {{ max-width:600px; margin:0 auto; background:white; border-radius:10px; overflow:hidden; box-shadow:0 4px 15px rgba(0,0,0,0.1); }}
        .header {{ background:#1a5fb4; color:white; padding:30px; text-align:center; }}
        .content {{ padding:40px 30px; text-align:center; }}
        .button {{ background:#1a5fb4; color:white; padding:16px 32px; text-decoration:none; border-radius:8px; font-size:18px; font-weight:bold; display:inline-block; margin:20px 0; }}
        .footer {{ background:#f8f9fa; padding:20px; font-size:12px; color:#666; text-align:center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>UniRumbo</h1>
        </div>
        <div class='content'>
            <h2>¿Olvidaste tu contraseña?</h2>
            <p>No te preocupes, pasa a cualquiera. Haz clic en el botón para crear una nueva:</p>
            <a href='{resetLink}' class='button'>Restablecer contraseña</a>
            <p>Este enlace expirará en <strong>30 minutos</strong> por seguridad.</p>
            <p>Si no solicitaste este cambio, puedes ignorar este correo.</p>
        </div>
        <div class='footer'>
            © 2025 UniRumbo - Universidad de Cundinamarca<br>
            Todos los derechos reservados
        </div>
    </div>
</body>
</html>"
            };

            message.To.Add(toEmail);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}