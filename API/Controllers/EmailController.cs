using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace API.Controllers
{
    public class EmailModel
    {
        public string FromEmail { get; set; } = string.Empty;
        public string ToEmails { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("SendEmail")]
        public ActionResult SendEmail(EmailModel emailData)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(emailData.FromEmail),
                Subject = emailData.Subject,
                IsBodyHtml = true,
                Body = $"""
                <html>
                    <body>
                        <h3>{emailData.Body}</h3>
                    </body>
                </html>
                """
            };
            foreach(var toEmail in emailData.ToEmails.Split(";"))
            {
                message.To.Add(new MailAddress(toEmail));
            }

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailData.FromEmail, "your key"),
                EnableSsl = true,
            };

            smtp.Send(message);

            return Ok("Email Sent!");
        }
    }
}
