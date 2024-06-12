using DienstDuizend.BookingService.Common.Interfaces;
using DienstDuizend.BookingService.Common.ValueObjects;
using DienstDuizend.BookingService.Features.Bookings.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain.Enums;
using DienstDuizend.BookingService.Infrastructure.Exceptions;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.GetById;

[Handler]
public static partial class GetById
{
    public record Query(Guid Id);
    public record Response(
        Guid Id, 
        Guid CustomerId,
        Guid BusinessId, 
        string Business, 
        string ServiceName, 
        PostalAddress AppointmentLocation,
        DateTime BookingDate, 
        BookingStatus BookingStatus,
        MoneyAmount Costs
    );
    
    private static async ValueTask<Response> HandleAsync(
        Query request,
        ApplicationDbContext dbContext,
        ICurrentUserProvider currentUserProvider,
        CancellationToken token)
    {
        Booking? booking = await dbContext.Bookings
            .Include(b => b.Business)
            .Include(b => b.Service)
            .FirstOrDefaultAsync(b => b.Id == request.Id, token);
        
        if (booking is null) throw Error.NotFound<Booking>();
        
        // If the current user is not the customer or the business owner, then error.
        if (!new []{booking.CustomerId, booking.Business.OwnerId}.Contains(currentUserProvider.GetCurrentUserId()))
            throw Error.Forbidden("User.NoPermission", "You do not have the right permissions to retrieve this booking.");

        return new Response(
            booking.Id,
            booking.CustomerId,
            booking.BusinessId,
            booking.Business.Name,
            booking.Service.Title,
            booking.AppointmentLocation,
            booking.BookingDate, booking.Status, 
            booking.Costs
            );
    }
}

