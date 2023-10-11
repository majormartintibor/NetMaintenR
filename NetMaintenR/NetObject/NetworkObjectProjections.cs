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
    Component[] Components
    //,string testField
    )
{
    public PoleDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue, 
              Array.Empty<Component>()
              //,string.Empty
              )
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
                    Array.Empty<Component>()
                    //,"It works!"
                    ),

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
    DateTimeOffset LastMajorInspection
    //,string testField
    )
{
    public SectionDetails() 
        : this(Guid.Empty, 
              DateTimeOffset.MinValue, 
              DateTimeOffset.MinValue
              //,string.Empty
              )
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
                    DateTimeOffset.MinValue
                    //,"It works!"
                    ),

            LastMinorInspectionUpdated(var inspectionTime)
                => current with { LastMinorInspection = inspectionTime },

            LastMajorInspectionUpdated(var inspectionTime)
                => current with { LastMajorInspection = inspectionTime },            

            _ => null
        };
}

public static class NetworkObjectStoreOptionsExtension
{
    public static void AddNetworkObjectProjections(this StoreOptions options)
    {
        //options.Projections.LiveStreamAggregation<NetworkObject>();
        options.Projections.Add<NetworkObjectInspectionDateDetailsProjection>(ProjectionLifecycle.Live);
        options.Projections.Add<PoleDetailsProjection>(ProjectionLifecycle.Live);
        options.Projections.Add<SectionDetailsProjection>(ProjectionLifecycle.Live);
    }
}