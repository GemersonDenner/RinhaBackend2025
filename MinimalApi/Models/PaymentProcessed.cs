namespace MinimalApi.Models;

public record PaymentProcessed()
{
    public string correlationId { get; set; } = string.Empty;
    public decimal amount { get; set; } = 0;
    public DateTime processedAt { get; set; } = DateTime.UtcNow;
}