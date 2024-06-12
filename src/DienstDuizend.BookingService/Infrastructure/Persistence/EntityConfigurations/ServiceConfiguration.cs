using DienstDuizend.BookingService.External.Domain;
using DienstDuizend.BookingService.Features.Bookings.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyAmount = DienstDuizend.BookingService.Common.ValueObjects.MoneyAmount;

namespace DienstDuizend.BookingService.Infrastructure.Persistence.EntityConfigurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(e => e.Price)
            .HasConversion(new MoneyAmount.EfCoreValueConverter());
    }
}