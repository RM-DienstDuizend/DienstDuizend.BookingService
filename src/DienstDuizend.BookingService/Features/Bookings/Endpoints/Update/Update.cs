using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.Update;

[ApiController, Route("/bookings/{Id}")]
[Authorize]
public class UpdateEndpoint(Update.Handler handler) : ControllerBase
{
    [HttpPut]
    public async Task<Update.Response> HandleAsync(
        [FromRoute] Guid Id,
        [FromBody] Update.Command request,
        CancellationToken cancellationToken = new()
    ) => await handler.HandleAsync(request with {Id = Id}, cancellationToken);
}