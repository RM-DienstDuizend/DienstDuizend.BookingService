using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using DienstDuizend.Events;
using MassTransit;

namespace DienstDuizend.BookingService.External.Consumers;

public class BusinessCreatedConsumer(ApplicationDbContext dbContext) : IConsumer<BusinessCreatedEvent>
{
    public async Task Consume(ConsumeContext<BusinessCreatedEvent> context)
    {
        var businessDto = context.Message;

        await dbContext.Businesses.AddAsync(new Business()
        {
            BusinessId = businessDto.Id,
            OwnerId = businessDto.UserId,
            Name = businessDto.Name
        });

        await dbContext.SaveChangesAsync();

    }
}