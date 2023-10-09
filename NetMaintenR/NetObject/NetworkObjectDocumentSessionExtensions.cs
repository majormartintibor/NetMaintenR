using Marten;
using NetMaintenR.Shared;
using static NetMaintenR.NetObject.NetworkObject;

namespace NetMaintenR.NetObject;

public static class NetworkObjectDocumentSessionExtensions
{
    public static Task Decide(
        this IDocumentSession session,
        Guid streamId,
        NetworkObjectCommand command,
        CancellationToken ct = default
        ) =>
            session.Decide<NetworkObject, NetworkObjectCommand, NetworkObjectEvent>(
                (c, s) => new[] { NetworkObjectService.Decide(c, s) },
                () => new UndefinedNetworkObject(),
                streamId,
                command,
                ct);
}