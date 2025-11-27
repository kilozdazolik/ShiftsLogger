namespace ShiftsLogger.API.Models;

public class Shift
{
    public Guid Id { get; set; }
    public DateTime StartTime  { get; set; }
    public DateTime? EndTime { get; set; }
}