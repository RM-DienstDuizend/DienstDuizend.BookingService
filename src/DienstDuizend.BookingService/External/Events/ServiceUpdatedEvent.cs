namespace DienstDuizend.Events;

public record ServiceUpdatedEvent(
    Guid Id,
    string Title,
    string? Description,
    Guid BusinessId,
    decimal Price,
    int EstimatedDurationInMinutes,
    bool IsHomeService,
    bool IsPubliclyVisible
);