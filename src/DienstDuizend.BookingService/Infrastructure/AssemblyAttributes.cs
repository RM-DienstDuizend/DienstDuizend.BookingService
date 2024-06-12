using DienstDuizend.BookingService.Infrastructure.Behaviors;
using Immediate.Handlers.Shared;
using Vogen;

[assembly: 
    VogenDefaults(conversions: Conversions.TypeConverter | Conversions.SystemTextJson | Conversions.EfCoreValueConverter)
]

[assembly: Behaviors(
    typeof(ValidatorPreProcessor<,>)
)]