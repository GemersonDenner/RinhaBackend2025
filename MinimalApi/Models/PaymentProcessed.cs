namespace MinimalApi.Models;

public record PaymentProcessed()
{
    public Guid? correlationId { get; set; } = null;
    public decimal amount { get; set; } = 0;
    public DateTime processedAt { get; set; } = DateTime.UtcNow;
}