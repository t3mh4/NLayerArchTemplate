using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLayerArchTemplate.DataAccess;
using NLayerArchTemplate.DataAccess.Repositories;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using Scrutor;
using NLayerArchTemplate.Business.Abstracts;

namespace NLayerArchTemplate.Business;


public static class DependencyResolver
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, bool isDevelopment,
                              string connectionString)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(connectionString,
                             ServerVersion.AutoDetect(connectionString),
                             x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (isDevelopment)
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
            }
        });
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddValidatorsFromAssembly(typeof(DependencyResolver).Assembly); // register validators
        services.AddAutoMapper(typeof(DependencyResolver).Assembly); // register automapper

        services.Scan(scan =>
        {
            scan.FromAssemblies(typeof(Repository<>).Assembly)
                .AddClasses(x => x.AssignableTo(typeof(IRepository<>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime();
        });

        services.Scan(scan =>
        {
            scan.FromAssemblies(typeof(ABaseManager).Assembly)
                .AddClasses(x => x.AssignableTo(typeof(ABaseManager)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime();
        });

        return services;
    }
}

