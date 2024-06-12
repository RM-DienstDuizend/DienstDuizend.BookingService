using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using DienstDuizend.Events;
using MassTransit;
using MoneyAmount = DienstDuizend.BookingService.Common.ValueObjects.MoneyAmount;

namespace DienstDuizend.BookingService.External.Consumers;

public class ServiceCreatedConsumer(ApplicationDbContext dbContext) : IConsumer<ServiceCreatedEvent>
{
    public async Task Consume(ConsumeContext<ServiceCreatedEvent> context)
    {
        var serviceDto = context.Message;

        await dbContext.Services.AddAsync(new Service()
        {
            ServiceId = serviceDto.Id,
            Title = serviceDto.Title,
            BusinessId = serviceDto.BusinessId,
            IsHomeService = serviceDto.IsHomeService,
            Price = MoneyAmount.From(serviceDto.Price),
            EstimatedDurationInMinutes = serviceDto.EstimatedDurationInMinutes
        });

        await dbContext.SaveChangesAsync();

    }
}