using DienstDuizend.BookingService.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.GetAllWithPagination;

[ApiController, Route("/bookings")]
[Authorize]
public class GetAllForCustomerWithPaginationEndpoint(GetAllWithPagination.Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResult<GetAllWithPagination.Response>> HandleAsync(
        [FromQuery] GetAllWithPagination.Query request,
        CancellationToken cancellationToken = new()
    ) => await handler.HandleAsync(request, cancellationToken);
}