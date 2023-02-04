namespace Wallabag.Options;

public class WorkerSettings
{
    public const string Position = "Worker";

    public int Interval { get; set; } = 0;
    
    public string SyncTag { get; set; } = "readlater";
}