namespace NetMaintenR.NetInspectR;

public record NetworkInspection
{
    public Guid Id { get; set; }

    public bool Closed { get; set; } = false;

    private NetworkInspection() { }

    public record Open() : NetworkInspection;

    public record Close() : NetworkInspection;
}
