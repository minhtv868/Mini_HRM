using Web.Application.Interfaces;

namespace Web.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.Now;
    }
}