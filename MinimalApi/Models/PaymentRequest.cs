namespace MinimalApi.Models;

public record PaymentRequest(string correlationId, decimal amount);
