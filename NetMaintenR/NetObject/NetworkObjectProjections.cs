﻿using Marten;
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
        : this(Guid.Empty, DateTimeOffset.MinValue, DateTimeOffset.MinValue)
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

public static class NetworkObjectStoreOptionsExtension
{
    public static void AddNetworkObjectProjections(this StoreOptions options)
    {
        options.Projections.Add<NetworkObjectInspectionDateDetailsProjection>(ProjectionLifecycle.Inline);
    }
}