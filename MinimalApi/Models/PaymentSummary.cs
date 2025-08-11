namespace MinimalApi.Models;

public record PaymentSummary()
{
    public SummaryInfo Default { get; set; } = new SummaryInfo();
    public SummaryInfo Fallback { get; set; } = new SummaryInfo();
};

public record SummaryInfo
{
    public int totalRequests { get; set; }
    public decimal totalAmount { get; set; }

    public SummaryInfo() : this(0, 0) { }

    public SummaryInfo(int totalRequests, decimal totalAmount)
    {
        this.totalRequests = totalRequests;
        this.totalAmount = totalAmount;
    }
}
