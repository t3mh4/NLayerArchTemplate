using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLayerArchTemplate.DataAccess;
using NLayerArchTemplate.DataAccess.Repositories;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using NLayerArchTemplate.DataAccess.Services.UserService;
using NtierArchTemplate.Business.UserManager;
using NtierArchTemplate.DataAccess.Services.UserService;

namespace NtierArchTemplate.Business;

public static class DependencyResolver
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, bool isDevelopment,
                              string connectionString)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            if (isDevelopment)
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
            }
        });
        services.AddTransient<IUserManager, UserManager.UserManager>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddValidatorsFromAssembly(typeof(DependencyResolver).Assembly); // register validators
        services.AddAutoMapper(typeof(DependencyResolver).Assembly); // register automapper
        return services;
    }
}
