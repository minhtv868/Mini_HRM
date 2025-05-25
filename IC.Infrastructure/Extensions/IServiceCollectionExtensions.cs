using IC.Application.Interfaces;
using IC.Domain.Common;
using IC.Domain.Common.Interfaces;
using IC.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace IC.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<ICurrentUserService, CurrentUserService>();
        }
    }
}
