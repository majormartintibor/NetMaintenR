using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using NetMaintenR.Shared;
using static NetMaintenR.NetObject.NetworkObjectCommand;
namespace NetMaintenR.NetObject;

public static class Endpoints
{
    public static void MapNetObjectEndpoints(this WebApplication app)
    {
        var netObject = app.MapGroup("/netObject").WithOpenApi();

        netObject.MapPost(
            "/Pole",
            async Task<Results<Ok<Guid>, BadRequest<string>>> (                
                IDocumentSession session
                ) =>
            {
                try
                {
                    var id = Guid.NewGuid();

                    await session.Decide(id, new CreatePole(), CancellationToken.None);
                    return TypedResults.Ok(id);
                }
                catch(Exception ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
            }).WithDisplayName("CreatePole");

        netObject.MapPost(
            "/Section",
            async Task<Results<Ok<Guid>, BadRequest<string>>> (                
                IDocumentSession session
                ) =>
            {
                try
                {
                    var id = Guid.NewGuid();

                    await session.Decide(id, new CreateSection(), CancellationToken.None);
                    return TypedResults.Ok(id);
                }
                catch(Exception ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
            }).WithDisplayName("CreateSection");

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
            }).WithDisplayName("UpdateMinorInspectionDate");

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
            }).WithDisplayName("UpdateMajorInspectionDate");

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

        netObject.MapGet(
            "/",
            async Task<Results<Ok<IReadOnlyList<NetworkObject>>, BadRequest>> (
                IDocumentSession session
                ) =>
            {
                try
                {
                    var networkObjects = await session.Query<NetworkObject>().ToListAsync();
                    return TypedResults.Ok(networkObjects);
                }
                catch
                {
                    return TypedResults.BadRequest();
                }               
            });

        netObject.MapGet( //152cf5bb-bfbf-4cc4-b184-9d818683dfc9
            "/{id}",
            async Task<Results<Ok<NetworkObject>, BadRequest>> (
                Guid id,
                IDocumentSession session
                ) =>
            {
                try
                {
                    var networkObject = await session.Get<NetworkObject>(id, CancellationToken.None);
                    return TypedResults.Ok(networkObject);
                }
                catch
                {
                    return TypedResults.BadRequest();
                }                
            });
    }
}