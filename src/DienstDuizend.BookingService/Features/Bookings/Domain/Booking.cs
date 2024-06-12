using DienstDuizend.BookingService.Common.ValueObjects;
using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain.Enums;

namespace DienstDuizend.BookingService.Features.Bookings.Domain;

public class Booking
{
    // Ef core
    protected Booking() {}
    
    public Booking(Guid businessId, Guid serviceId, Guid customerId, PostalAddress appointmentLocation, DateTime bookingDate, MoneyAmount costs)
    {
        BusinessId = businessId;
        ServiceId = serviceId;
        CustomerId = customerId;
        AppointmentLocation = appointmentLocation;
        BookingDate = bookingDate;
        Costs = costs;
    }

    public Guid Id { get; init; }
    
    public Guid BusinessId { get; set; }
    public Business Business { get; set; }
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    public MoneyAmount Costs { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public PostalAddress AppointmentLocation { get; set; }
    
    public DateTime BookingDate { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.WaitingForApproval;
}