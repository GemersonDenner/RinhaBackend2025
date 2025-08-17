namespace MinimalApi.Models;

public record PaymentSummary()
{
    public SummaryInfo Default { get; set; } = new SummaryInfo();
    public SummaryInfo Fallback { get; set; } = new SummaryInfo();
};

public record SummaryInfo
{
    public int TotalRequests { get; set; }
    public decimal TotalAmount { get; set; }

    public SummaryInfo() : this(0, 0) { }

    public SummaryInfo(int totalRequests, decimal totalAmount)
    {
        this.TotalRequests = totalRequests;
        this.TotalAmount = totalAmount;
    }
}
