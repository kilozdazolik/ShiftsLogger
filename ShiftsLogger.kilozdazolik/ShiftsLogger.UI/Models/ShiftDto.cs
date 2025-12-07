namespace ShiftsLogger.UI.Models;

public class ShiftDto
{
    public Guid Id { get; set; }
    public DateTime StartTime  { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration { get; set; }
}