namespace DienstDuizend.BookingService.Common.Interfaces;

public interface ICurrentUserProvider
{
    public Guid GetCurrentUserId();
}