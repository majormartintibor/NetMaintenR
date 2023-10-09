using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using static NetMaintenR.NetObject.NetworkObjectCommand;
namespace NetMaintenR.NetObject;

public static class Endpoints
{
    public static void MapNetObjectEndpoints(this WebApplication app)
    {
        var netObject = app.MapGroup("/netObject").WithOpenApi();

        netObject.MapPut(
            "/UpdateMinorInspectionDate/{id}",
            async Task<Results<Ok, BadRequest>> (
                Guid id,
                UpdateLastMinorInspection command,
                IDocumentSession session
                ) =>
            {
                try
                {
                    await session.Decide(id, command, CancellationToken.None);
                    return TypedResults.Ok();
                }
                catch
                {
                    return TypedResults.BadRequest();
                }
            }).WithDisplayName("");

        netObject.MapPut(
            "/UpdateMajorInspectionDate/{id}",
            async Task<Results<Ok, BadRequest>> (
                Guid id,
                UpdateLastMajorInspection command,
                IDocumentSession session
                ) =>
            {
                try
                {
                    await session.Decide(id, command, CancellationToken.None);
                    return TypedResults.Ok();
                }
                catch
                {
                    return TypedResults.BadRequest();
                }
            }).WithDisplayName("");

        netObject.MapPut(
            "/AddComponent/{id}",
            async Task<Results<Ok, BadRequest>> (
                Guid id,
                AddComponent command,
                IDocumentSession session
                ) =>
            {
                try
                {
                    await session.Decide(id, command, CancellationToken.None);
                    return TypedResults.Ok();
                }
                catch
                {
                    return TypedResults.BadRequest();
                }
            }).WithDisplayName("AddComponent");

        netObject.MapPut(
            "/RemoveComponent/{id}",
            async Task<Results<Ok, BadRequest>> (
                Guid id,
                RemoveComponent command,
                IDocumentSession session
                ) =>
            {
                try
                {
                    await session.Decide(id, command, CancellationToken.None);
                    return TypedResults.Ok();
                }
                catch
                {
                    return TypedResults.BadRequest();
                }
            }).WithDisplayName("RemoveComponent");
    }
}