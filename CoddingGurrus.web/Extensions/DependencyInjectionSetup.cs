using CoddingGurrus.Core.Interface;
using CoddingGurrus.Infrastructure.CommonHelper.Handler;

namespace CoddingGurrus.web.Extensions
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfigurationRoot config)
        {
            return services
                .AddScoped<IBaseHandler, BaseHandler>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
