namespace NetMaintenR.NetWorkR;

public record Worker
{
    public Guid Id { get; set; }

    public bool CanDoMajorNetworkInspection { get; set; }
}
