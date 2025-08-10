namespace MinimalApi.Models;

public record PaymentSummary()
{
    public SummaryInfo Default { get; set; } = new SummaryInfo(0, 0);
    public SummaryInfo Fallback { get; set; } = new SummaryInfo(0, 0);
};

public record SummaryInfo(int totalRequests, decimal totalAmount);
