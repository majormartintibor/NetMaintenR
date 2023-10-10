using static NetMaintenR.NetObject.NetworkObject;
using static NetMaintenR.NetObject.NetworkObjectCommand;
using static NetMaintenR.NetObject.NetworkObjectEvent;

namespace NetMaintenR.NetObject;

public static class NetworkObjectService
{
    public static NetworkObjectEvent Decide(
        NetworkObjectCommand command,
        NetworkObject state
        ) =>
            command switch
            {
                CreatePole createPole => Handle(createPole),
                CreateSection createSection => Handle(createSection),
                UpdateLastMinorInspection updateLastMinorInspection => Handle(updateLastMinorInspection, state),
                UpdateLastMajorInspection updateLastMajorInspection => Handle(updateLastMajorInspection, state),
                AddComponent addComponent => Handle(addComponent, state.EnsureIsPole()),
                RemoveComponent removeComponent => Handle(removeComponent, state.EnsureIsPole()),
                _ => throw new InvalidOperationException($"Cannot handle {command.GetType().Name} command.")
            };

    public static PoleCreated Handle(CreatePole command)
        => new(command.Id);

    public static SectionCreated Handle(CreateSection command)
        => new(command.Id);

    public static LastMinorInspectionUpdated Handle(UpdateLastMinorInspection command, NetworkObject networkObject)
        => new(command.InspectionTime);

    public static LastMajorInspectionUpdated Handle(UpdateLastMajorInspection command, NetworkObject networkObject)
        => new(command.InspectionTime);

    public static ComponentAdded Handle(AddComponent command, Pole pole)
        => new(command.Component);

    public static ComponentRemoved Handle(RemoveComponent command, Pole pole)
        => new(command.CompontentId);    

    private static Pole EnsureIsPole(this NetworkObject networkObject) =>
        networkObject as Pole ?? throw new InvalidOperationException(
            $"Invalid operation for '{networkObject.GetType().Name}' network object.");
}


public abstract record NetworkObjectCommand
{
    public record CreatePole(Guid Id) : NetworkObjectCommand;

    public record CreateSection(Guid Id) : NetworkObjectCommand;

    public record UpdateLastMinorInspection(DateTimeOffset InspectionTime) : NetworkObjectCommand;

    public record UpdateLastMajorInspection(DateTimeOffset InspectionTime) : NetworkObjectCommand;

    public record AddComponent(Component Component) : NetworkObjectCommand;

    public record RemoveComponent(Guid CompontentId) : NetworkObjectCommand;
}