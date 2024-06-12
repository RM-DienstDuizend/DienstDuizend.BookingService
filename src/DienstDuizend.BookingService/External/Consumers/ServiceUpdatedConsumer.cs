using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using DienstDuizend.Events;
using MassTransit;
using MoneyAmount = DienstDuizend.BookingService.Common.ValueObjects.MoneyAmount;

namespace DienstDuizend.BookingService.External.Consumers;

public class ServiceUpdatedConsumer(ApplicationDbContext dbContext, ILogger<ServiceUpdatedConsumer> logger) 
    : IConsumer<ServiceUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ServiceUpdatedEvent> context)
    {
        var serviceDto = context.Message;

        var service = await dbContext.Services.FirstOrDefaultAsync(b => b.ServiceId == serviceDto.Id);

        if (service is null)
        {
            logger.LogError($"Tried to update service with id of '{serviceDto.Id}', however this service doesn't exist.");
            return;
        }

        service.Title = serviceDto.Title;
        service.IsHomeService = serviceDto.IsHomeService;
        service.Price = MoneyAmount.From(serviceDto.Price);
        service.EstimatedDurationInMinutes = serviceDto.EstimatedDurationInMinutes;

        await dbContext.SaveChangesAsync();

    }
}