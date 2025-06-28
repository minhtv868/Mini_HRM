using Web.Application.Interfaces;
using Web.Domain.Common;
using Web.Domain.Common.Interfaces;
using Web.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Web.Infrastructure.Extensions
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
