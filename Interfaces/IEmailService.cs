using System.Threading.Tasks;

namespace LSA.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string emailTo, string subject, string body);
}
