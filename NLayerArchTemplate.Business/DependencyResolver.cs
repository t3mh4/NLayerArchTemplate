using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLayerArchTemplate.DataAccess;
using NLayerArchTemplate.DataAccess.Repositories;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;

namespace NtierArchTemplate.Business;

public static class DependencyResolver
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
            }
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddValidatorsFromAssembly(typeof(DependencyResolver).Assembly); // register validators
        services.AddAutoMapper(typeof(DependencyResolver).Assembly); // register automapper
        return services;
    }
}
