using FluentValidation;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.Create;

public class CreateValidator : AbstractValidator<Create.Command>
{
    public CreateValidator()
    {
    }       
}
