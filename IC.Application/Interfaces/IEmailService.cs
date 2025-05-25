using IC.Application.DTOs.Email;

namespace IC.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}
