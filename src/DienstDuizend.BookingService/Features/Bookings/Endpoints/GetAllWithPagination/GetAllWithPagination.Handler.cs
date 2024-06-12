using DienstDuizend.BookingService.Common.Dto;
using DienstDuizend.BookingService.Common.Extensions;
using DienstDuizend.BookingService.Common.Interfaces;
using DienstDuizend.BookingService.Common.ValueObjects;
using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain.Enums;
using DienstDuizend.BookingService.Infrastructure.Exceptions;
using DienstDuizend.BookingService.Infrastructure.Persistence;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.GetAllWithPagination;

[Handler]
public static partial class GetAllWithPagination
{
    public record Query(Guid? BusinessId, int PageSize = 100, int PageIndex = 1);

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

    private static async ValueTask<PaginationResult<Response>> HandleAsync(
        Query request,
        ApplicationDbContext dbContext,
        ICurrentUserProvider currentUserProvider,
        CancellationToken token)
    {

        var requestMadeByBusinessOwner = false;
        
        if (request.BusinessId is not null)
        {
            var business = await dbContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId);
            if (business is null) throw Error.NotFound<Business>();

            if (currentUserProvider.GetCurrentUserId() == business.BusinessId) requestMadeByBusinessOwner = true;
        }

        var bookings = await dbContext.Bookings
                .Include(b => b.Business)
                .Include(b => b.Service)
                .WhereIf(requestMadeByBusinessOwner, b => b.BusinessId == request.BusinessId)
            .Paginate(request.PageIndex, request.PageSize)
            .Select(x => new Response(
                x.Id,
                x.CustomerId,
                x.BusinessId,
                x.Business.Name, 
                x.Service.Title,
                x.AppointmentLocation,
                x.BookingDate,
                x.Status,
                x.Costs
                ))
            .ToListAsync(token);

        return new PaginationResult<Response>
        {
            Data = bookings,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalRecords = await dbContext.Bookings.CountAsync(token)
        };
    }
}