using LSA.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LSA.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string emailTo, string subject, string body)
    {
        using var client = new SmtpClient("smtp.example.com");
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("username", "password");

        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("sender@example.com");
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.To.Add(emailTo);

        await Task.Run(() => client.SendAsync(mailMessage, null)).ConfigureAwait(false);

    }
}
