using DienstDuizend.BookingService.Common.Interfaces;
using DienstDuizend.BookingService.Infrastructure.Exceptions.Handlers;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using DienstDuizend.BookingService.Infrastructure.Services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;

namespace DienstDuizend.BookingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHandlers();
        services.AddBehaviors();

        services.AddExceptionHandler<ApplicationExceptionHandler>();
        services.AddExceptionHandler<FluentValidationExceptionHandler>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                // Metrics provider from OpenTelemetry
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                // Metrics provides by ASP.NET Core in .NET 8
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddPrometheusExporter()); // We use v1.7 because currently v1.8 has an issue with formatting.


        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.AddConsumers(typeof(IAssemblyMarker).Assembly);
            
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("MessageBroker"));
                
                configurator.ConfigureEndpoints(context);
            });
        });

        // serviceCollection.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        
        return services;
    }
}