
using DienstDuizend.BookingService.Common.ValueObjects;

namespace DienstDuizend.BookingService.External.Domain;


// Not the source of truth, this data is the necessary data received from the business catalogue service.
public class Service
{
    public Guid ServiceId { get; set; }
    public string Title { get; set; }
    public int EstimatedDurationInMinutes { get; set; }
    public MoneyAmount Price { get; set; }
    public bool IsHomeService { get; set; }
    public Guid BusinessId { get; set; }

}