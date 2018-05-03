using System.Threading.Tasks;

namespace MedicalClinic.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
