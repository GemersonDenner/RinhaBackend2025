namespace MinimalApi.Models;

public record PaymentRequest(Guid correlationId, decimal amount);
