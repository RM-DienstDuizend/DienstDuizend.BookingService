namespace DienstDuizend.BookingService.Features.Bookings.Domain;

public class PostalAddress{
    public string StreetAddress { get; set; }
    public string? StreetAddress2 { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}