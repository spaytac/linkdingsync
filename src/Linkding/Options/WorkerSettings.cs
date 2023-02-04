namespace Linkding.Options;

public class WorkerSettings
{
    public const string Position = "Worker";

    public int Interval { get; set; } = 0;
    public int TagNameLength { get; set; } = 64;
    public List<string> Tasks { get; set; } = new List<string>();
}