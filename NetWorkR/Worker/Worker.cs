namespace NetWorkR.Worker;

public record Worker
{
    public Guid Id { get; set; }

    public bool CanDoMajorNetworkInspection { get; set; }
}