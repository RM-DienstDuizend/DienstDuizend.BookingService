using System.ComponentModel.DataAnnotations;

namespace DienstDuizend.BookingService.External.Domain;


// Not the source of truth, this data is the necessary data received from the business catalogue service.
public class Business
{
    [Key]
    public Guid BusinessId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
}