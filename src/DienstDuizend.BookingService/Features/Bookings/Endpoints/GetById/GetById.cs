using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.GetById;

[ApiController, Route("/bookings/{Id}")]
[Authorize]
public class GetByIdEndpoint(GetById.Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<GetById.Response> HandleAsync(
        [FromRoute] GetById.Query request,
        CancellationToken cancellationToken = new()
    ) => await handler.HandleAsync(request, cancellationToken);
}