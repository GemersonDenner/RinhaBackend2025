using System.Text.Json.Serialization;

namespace MinimalApi.Models;

public record PaymentSummary()
{
    [JsonPropertyName("default")]
    public SummaryInfo Default { get; set; } = new SummaryInfo();

    [JsonPropertyName("fallback")]
    public SummaryInfo Fallback { get; set; } = new SummaryInfo();
};

public record SummaryInfo
{
    [JsonPropertyName("totalRequests")]
    public int TotalRequests { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    public SummaryInfo() : this(0, 0) { }

    public SummaryInfo(int totalRequests, decimal totalAmount)
    {
        this.TotalRequests = totalRequests;
        this.TotalAmount = totalAmount;
    }
}
