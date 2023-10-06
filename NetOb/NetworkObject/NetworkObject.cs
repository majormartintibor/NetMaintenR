namespace NetOb.NetworkObject;

public record NetworkObject
{
    public Guid Id { get; set; }

    public DateTime LastInspected 
        => LastMajorInspection > LastMinorInspection ? LastMajorInspection : LastMinorInspection;

    public DateTime LastMinorInspection { get; set; }

    public DateTime LastMajorInspection { get; set; }

    private NetworkObject() { }

    public record Pole(List<Component> Components): NetworkObject;

    public record Section: NetworkObject;
}

public record Component(Guid Id, string Name);

public abstract record NetworkObjectCommand
{
    public record CreatePole(Guid Id, List<Component> Components) : NetworkObjectCommand;

    public record CreateSection(Guid Id) : NetworkObjectCommand;
}