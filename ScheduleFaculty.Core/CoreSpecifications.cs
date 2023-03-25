//using ScheduleFaculty.Core.Services;
//using ScheduleFaculty.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ScheduleFaculty.Core.Services;
using ScheduleFaculty.Core.Services.Abstractions;


namespace ScheduleFaculty.Core;

public static class CoreSpecifications
{
    public static IServiceCollection AddCoreSpecifications(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}