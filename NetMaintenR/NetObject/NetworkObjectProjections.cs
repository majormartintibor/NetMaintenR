using JasperFx.CodeGeneration.Frames;
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

public class PoleDetailsProjection 
    : SingleStreamProjection<PoleDetails>
{
    public PoleDetails? Apply(
        NetworkObjectEvent @event, PoleDetails current) =>
        @event switch
        {
            PoleCreated(var id)
                => new PoleDetails(
                    id, 
                    DateTimeOffset.MinValue, 
                    DateTimeOffset.MinValue, 
                    Array.Empty<Component>()),

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
}

public class SectionDetailsProjection
    : SingleStreamProjection<SectionDetails>
{
    public SectionDetails? Apply(
        NetworkObjectEvent @event, SectionDetails current) =>
        @event switch
        {
            SectionCreated(var id)
                => new SectionDetails(
                    id, 
                    DateTimeOffset.MinValue, 
                    DateTimeOffset.MinValue),

            LastMinorInspectionUpdated(var inspectionTime)
                => current with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => current with { LastMajorInspection = inspectionTime },            

            _ => null
        };
}

public record SampleLive(
    Guid Id,
    DateTimeOffset LastMinorInspection,
    DateTimeOffset LastMajorInspection
    , string SampleLiveProjectionExtension
    )
{
    public SampleLive() 
        : this(Guid.Empty,
              DateTimeOffset.MinValue,
              DateTimeOffset.MinValue
              ,string.Empty
              )
    {
        
    }

    public SampleLive Apply(NetworkObjectEvent @event) =>
        @event switch
        {
            PoleCreated(var id)
                => new SampleLive(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue
                    , "Hey it works!"
                    ),

            SectionCreated(var id)
                => new SampleLive(
                    id,
                    DateTimeOffset.MinValue,
                    DateTimeOffset.MinValue
                    , "Hey it works!"
                    ),

            LastMinorInspectionUpdated(var inspectionTime)
                => this with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => this with { LastMajorInspection = inspectionTime },

            _ => this
        };
}

public class SampleLiveProjection
    : SingleStreamProjection<SampleLive>
{
    public SampleLive Apply(
        NetworkObjectEvent @event, SampleLive current) =>
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
        options.Projections.Add<SampleLiveProjection>(ProjectionLifecycle.Inline);
    }
}