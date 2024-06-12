using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using DienstDuizend.Events;
using MassTransit;

namespace DienstDuizend.BookingService.External.Consumers;

public class BusinessUpdatedConsumer(ApplicationDbContext dbContext, ILogger<BusinessUpdatedConsumer> logger) 
    : IConsumer<BusinessUpdatedEvent>
{
    public async Task Consume(ConsumeContext<BusinessUpdatedEvent> context)
    {
        var businessDto = context.Message;

        var business = await dbContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessDto.Id);

        if (business is null)
        {
            logger.LogError($"Tried to update business with id of '{businessDto.Id}', however this business doesn't exist.");
            return;
        }

        business.Name = businessDto.Name;
        business.OwnerId = businessDto.UserId;

        await dbContext.SaveChangesAsync();

    }
}