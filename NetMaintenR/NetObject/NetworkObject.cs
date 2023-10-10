using JasperFx.Core;
using static NetMaintenR.NetObject.NetworkObjectEvent;

namespace NetMaintenR.NetObject;

public record NetworkObject
{
    public NetworkObject Apply(NetworkObjectEvent @event) =>
        @event switch
        {
            PoleCreated(var id)
                => new Pole(Array.Empty<Component>()),

            SectionCreated(var id)
                => new Section(),

            LastMinorInspectionUpdated(var inspectionTime)
                => this with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => this with { LastMajorInspection = inspectionTime },

            ComponentAdded(var component)
                => this is Pole pole
                    ? pole with
                    {
                        Components = pole.Components
                        .Concat(new[] { component })
                        .ToArray()
                    }
                    : this,

            ComponentRemoved(var componentId)
                => this is Pole pole
                    ? pole with
                    {
                        Components = pole.Components
                        .Remove(pole.Components.Single(c => c.ComponentId == componentId))
                    }
                    : this,

            _ => this
        };

    public Guid Id { get; set; }    

    public DateTimeOffset LastInspected
        => LastMajorInspection > LastMinorInspection ? LastMajorInspection : LastMinorInspection;

    public DateTimeOffset LastMinorInspection { get; set; }

    public DateTimeOffset LastMajorInspection { get; set; }

    private NetworkObject() { }

    public record UndefinedNetworkObject : NetworkObject;

    public record Pole(Component[] Components) : NetworkObject;

    public record Section : NetworkObject;
}
public record Component(Guid ComponentId, string Name);


public abstract record NetworkObjectEvent
{
    public record PoleCreated(Guid Id) : NetworkObjectEvent;

    public record SectionCreated(Guid Id) : NetworkObjectEvent;

    public record LastMinorInspectionUpdated(DateTimeOffset InspectionTime) : NetworkObjectEvent;

    public record LastMajorInspectionUpdated(DateTimeOffset InspectionTime) : NetworkObjectEvent;

    public record ComponentAdded(Component Component) : NetworkObjectEvent;

    public record ComponentRemoved(Guid ComponentId) : NetworkObjectEvent;
}