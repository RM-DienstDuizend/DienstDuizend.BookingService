namespace DienstDuizend.Events;

public record BusinessUpdatedEvent(
    Guid Id,
    string Name,
    string? Description,
    string KvkNumber,
    string? BusinessEmail,
    Uri? WebsiteUri,
    Uri? LogoUri,
    Uri? BannerUri,
    Guid UserId    
);