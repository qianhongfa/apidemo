namespace MiniApp.Models;

public class OperationLog
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
