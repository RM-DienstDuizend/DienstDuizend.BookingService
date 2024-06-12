using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DienstDuizend.BookingService.Features.Bookings.Endpoints.Create;

[ApiController, Route("/bookings")]
[Authorize]
public class CreateEndpoint(Create.Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<Create.Response> HandleAsync(
        [FromBody] Create.Command request,
        CancellationToken cancellationToken = new()
    ) => await handler.HandleAsync(request, cancellationToken);
}