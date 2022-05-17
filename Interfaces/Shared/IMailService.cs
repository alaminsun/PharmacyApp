using StoreManager.Application.DTOs.Mail;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}