using DienstDuizend.BookingService.Common.Interfaces;
using DienstDuizend.BookingService.Features.Bookings.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain.Enums;
using DienstDuizend.BookingService.Infrastructure.Exceptions;
using DienstDuizend.BookingService.Infrastructure.Persistence;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.Update;

[Handler]
public static partial class Update
{
    public record Command(Guid Id, BookingStatus Status);

    public record Response;

    private static async ValueTask<Response> HandleAsync(
        Command request,
        ApplicationDbContext dbContext,
        ICurrentUserProvider currentUserProvider,
        CancellationToken token)
    {
        Booking? booking = await dbContext.Bookings
            .Include(b => b.Business)
            .FirstOrDefaultAsync(b => b.Id == request.Id, token);
        if (booking is null) throw Error.NotFound<Booking>();
        
        var currentUserId = currentUserProvider.GetCurrentUserId();

        if (booking.CustomerId == currentUserId)
        {
            if (request.Status != BookingStatus.Cancelled)
                throw Error.Failure("User.CanOnlyCancel", "As customer you can only cancel the booking.");

            if (booking.Status is BookingStatus.Completed or BookingStatus.Cancelled)
                throw Error.Failure("Booking.UnableToCancel", "This booking cannot be canceled anymore.");

            booking.Status = request.Status;
        } 
        else if (booking.Business.OwnerId == currentUserId)
        {
            if (booking.Status is BookingStatus.Completed or BookingStatus.Cancelled)
                throw Error.Failure("Booking.UnableToCancel", "This booking cannot be canceled anymore.");

            booking.Status = request.Status;
        }
        else
        {
            throw Error.Forbidden(
                "User.NoPermission", 
                "You do not have the right permissions to update this booking."
                );
        }
        
        await dbContext.SaveChangesAsync(token);

        return new Response();
    }
}

