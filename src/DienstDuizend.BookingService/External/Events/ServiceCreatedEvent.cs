namespace DienstDuizend.Events;

public record ServiceCreatedEvent(
    Guid Id,
    string Title,
    string? Description,
    Guid BusinessId,
    decimal Price,
    int EstimatedDurationInMinutes,
    bool IsHomeService,
    bool IsPubliclyVisible
);