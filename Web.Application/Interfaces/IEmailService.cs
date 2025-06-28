using Web.Application.DTOs.Email;

namespace Web.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}
