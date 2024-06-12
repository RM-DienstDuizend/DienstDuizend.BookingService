using System.Runtime.InteropServices.JavaScript;
using DienstDuizend.BookingService.Common.Extensions;
using DienstDuizend.BookingService.Common.Interfaces;
using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain;
using DienstDuizend.BookingService.Infrastructure.Exceptions;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using Immediate.Handlers.Shared;
using MassTransit;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.Create;

[Handler]
public static partial class Create
{
    public record Command(Guid BusinessId, Guid ServiceId, DateTime BookingDate, PostalAddress AppointmentLocation);

    public record Response(
        string Message = "You created a booking successfully, however it is still waiting to be approved by the business."
    );
    
    private static async ValueTask<Response> HandleAsync(
        Command request,
        ApplicationDbContext dbContext,
        ICurrentUserProvider currentUserProvider,
        CancellationToken token)
    {
        if (!await dbContext.Businesses.AnyAsync(b => b.BusinessId == request.BusinessId))
            throw Error.NotFound<Business>();

        var service = await dbContext.Services.FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId);
        if (service is null) throw Error.NotFound<Service>();
        
        // Check if booking date is available, by checking if the requested date is inside the time of any existing bookings
        if (await IsBookingOverlapAsync(dbContext.Bookings.Where(b => b.BusinessId == request.BusinessId),
                request.BookingDate))
        {
            throw Error.Failure(
                "Booking.UnavailableTime", 
                "The booking cannot be created, since the given date/time is unavailable."
                );
        }

        var booking = new Booking(
            request.BusinessId,
            request.ServiceId,
            currentUserProvider.GetCurrentUserId(),
            request.AppointmentLocation,
            request.BookingDate,
            service.Price
        );

        await dbContext.Bookings.AddAsync(booking, token);
        await dbContext.SaveChangesAsync(token);
        
        return new Response();
    }
    
    private static async Task<bool> IsBookingOverlapAsync(IQueryable<Booking> bookings, DateTime bookingDate)
    {
        return await bookings
            .Include(b => b.Service)
            .AnyAsync(b => 
                bookingDate.IsBetween(b.BookingDate, b.BookingDate.AddMinutes(b.Service.EstimatedDurationInMinutes))
                );
    }
}

