namespace DienstDuizend.BookingService.Common.Extensions;

public static class DateTimeExtensions
{
    public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime endDate)
    {
        // Ensure dates are using the same Kind (UTC or Local)
        if (dateTime.Kind != startDate.Kind)
        {
            dateTime = dateTime.ToUniversalTime();
            startDate = startDate.ToUniversalTime();
        }
        if (dateTime.Kind != endDate.Kind)
        {
            dateTime = dateTime.ToUniversalTime();
            endDate = endDate.ToUniversalTime();
        }

        return (dateTime >= startDate) && (dateTime < endDate);
    }
    
}