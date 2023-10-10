using JasperFx.Core;
using Marten;
using Marten.Events.Aggregation;
using Marten.Events.Projections;
using static NetMaintenR.NetObject.NetworkObjectEvent;

namespace NetMaintenR.NetObject;

public record NetworkObjectInspectionDateDetails(
    Guid Id,    
    DateTimeOffset LastMinorInspection,
    DateTimeOffset LastMajorInspection)
{
    public NetworkObjectInspectionDateDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue)
    {        
    }
};

public class NetworkObjectInspectionDateDetailsProjection 
    : SingleStreamProjection<NetworkObjectInspectionDateDetails>
{ 
    public NetworkObjectInspectionDateDetails Apply(
        NetworkObjectEvent @event, NetworkObjectInspectionDateDetails current) =>
        @event switch
        {
            PoleCreated(var id)
                => new NetworkObjectInspectionDateDetails(
                    id, DateTimeOffset.MinValue, DateTimeOffset.MinValue),

            SectionCreated(var id)
                => new NetworkObjectInspectionDateDetails(
                    id, DateTimeOffset.MinValue, DateTimeOffset.MinValue),

            LastMinorInspectionUpdated(var inspectionTime)
                => current with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => current with { LastMajorInspection = inspectionTime },

            _ => current
        };
}


public record PoleDetails(
    Guid Id,
    DateTimeOffset LastMinorInspection,
    DateTimeOffset LastMajorInspection,
    Component[] Components)
{
    public PoleDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue, 
              Array.Empty<Component>())
    {        
    }
}

//How to make this projection only for NetworkObjects that are Pole?
public class PoleDetailsProjection 
    : SingleStreamProjection<PoleDetails>
{
    public PoleDetails? Apply(
        NetworkObjectEvent @event, PoleDetails current) =>
        @event switch
        {
            PoleCreated(var id)
                => new PoleDetails(
                    id, DateTimeOffset.MinValue, DateTimeOffset.MinValue, Array.Empty<Component>()),

            LastMinorInspectionUpdated(var inspectionTime)
                => current with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => current with { LastMajorInspection = inspectionTime },

            ComponentAdded(var component)
            => current with
                {
                    Components = current.Components
                        .Concat(new[] { component })
                        .ToArray()
                },

            ComponentRemoved(var componentId)
                => current with
                    {
                        Components = current.Components
                        .Remove(current.Components.Single(c => c.ComponentId == componentId))
                    },

            _ => null
        };
}

public static class NetworkObjectStoreOptionsExtension
{
    public static void AddNetworkObjectProjections(this StoreOptions options)
    {
        options.Projections.Add<NetworkObjectInspectionDateDetailsProjection>(ProjectionLifecycle.Inline);
        options.Projections.Add<PoleDetailsProjection>(ProjectionLifecycle.Inline);
    }
}