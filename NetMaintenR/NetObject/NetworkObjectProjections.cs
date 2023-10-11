using JasperFx.Core;
using Marten;
using Marten.Events.Aggregation;
using Marten.Events.Projections;
using static NetMaintenR.NetObject.NetworkObjectEvent;

namespace NetMaintenR.NetObject;

public record NetworkObjectInspectionDateDetails(
    Guid Id,    
    DateTimeOffset LastMinorInspection,
    DateTimeOffset LastMajorInspection,
    NetworkObjectType NetworkObjectType)
{
    public NetworkObjectInspectionDateDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue,
              NetworkObjectType.Undefined)
    {        
    }

    public NetworkObjectInspectionDateDetails Apply(
        NetworkObjectEvent @event) =>
        @event switch
        {
            PoleCreated(var id)
                => new NetworkObjectInspectionDateDetails(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue,
                    NetworkObjectType.Pole),

            SectionCreated(var id)
                => new NetworkObjectInspectionDateDetails(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue,
                    NetworkObjectType.Section),

            LastMinorInspectionUpdated(var inspectionTime)
                => this with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => this with { LastMajorInspection = inspectionTime },

            _ => this
        };
};
public enum NetworkObjectType
{
    Undefined,
    Pole,
    Section
}

public class NetworkObjectInspectionDateDetailsProjection 
    : SingleStreamProjection<NetworkObjectInspectionDateDetails>
{
    public NetworkObjectInspectionDateDetails Apply(
        NetworkObjectEvent @event, NetworkObjectInspectionDateDetails current) =>
            current.Apply(@event);
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

    public PoleDetails? Apply(
        NetworkObjectEvent @event) =>
        @event switch
        {
            PoleCreated(var id)
                => new PoleDetails(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue,
                    Array.Empty<Component>()),

            LastMinorInspectionUpdated(var inspectionTime)
                => this with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => this with { LastMajorInspection = inspectionTime },

            ComponentAdded(var component)
                => this with
                {
                    Components = Components
                        .Concat(new[] { component })
                        .ToArray()
                },

            ComponentRemoved(var componentId)
                => this with
                {
                    Components = Components
                        .Remove(Components.Single(c => c.ComponentId == componentId))
                },

            _ => null
        };
}

public class PoleDetailsProjection 
    : SingleStreamProjection<PoleDetails>
{
    public PoleDetails? Apply(
        NetworkObjectEvent @event, PoleDetails current) =>
            current.Apply(@event);
}

public record SectionDetails(
    Guid Id,
    DateTimeOffset LastMinorInspection,
    DateTimeOffset LastMajorInspection)
{
    public SectionDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue)
    {        
    }

    public SectionDetails? Apply(
        NetworkObjectEvent @event) =>
        @event switch
        {
            SectionCreated(var id)
                => new SectionDetails(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue),

            LastMinorInspectionUpdated(var inspectionTime)
                => this with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => this with { LastMajorInspection = inspectionTime },

            _ => null
        };
}

public class SectionDetailsProjection
    : SingleStreamProjection<SectionDetails>
{
    public SectionDetails? Apply(
        NetworkObjectEvent @event, SectionDetails current) =>
            current.Apply(@event);
}

public static class NetworkObjectStoreOptionsExtension
{
    public static void AddNetworkObjectProjections(this StoreOptions options)
    {
        options.Projections.LiveStreamAggregation<NetworkObject>();
        options.Projections.Add<NetworkObjectInspectionDateDetailsProjection>(ProjectionLifecycle.Inline);
        options.Projections.Add<PoleDetailsProjection>(ProjectionLifecycle.Inline);
        options.Projections.Add<SectionDetailsProjection>(ProjectionLifecycle.Inline);        
    }
}