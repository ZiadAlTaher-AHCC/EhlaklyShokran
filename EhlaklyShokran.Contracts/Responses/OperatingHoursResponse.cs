namespace EhlaklyShokran.Contracts.Responses;

public sealed record OperatingHoursResponse(TimeOnly OpeningTime, TimeOnly ClosingTime);