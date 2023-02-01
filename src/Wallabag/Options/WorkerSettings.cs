namespace Wallabag.Options;

public class WorkerSettings
{
    public const string Position = "Worker";

    public int Intervall { get; set; } = 0;
    
    public string SyncTag { get; set; } = "readlater";
}