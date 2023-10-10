using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
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

                    await session.Decide(id, new CreatePole(id), CancellationToken.None);
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

                    await session.Decide(id, new CreateSection(id), CancellationToken.None);
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
            async Task<Results<Ok<IReadOnlyList<NetworkObjectInspectionDateDetails>>, BadRequest>> (
                IQuerySession querySession
                ) =>
            {
                try
                {
                    var networkObjects = await querySession
                        .Query<NetworkObjectInspectionDateDetails>()
                        .ToListAsync(CancellationToken.None);
                    return TypedResults.Ok(networkObjects);
                }
                catch
                {
                    return TypedResults.BadRequest();
                }               
            });
        
        netObject.MapGet(
            "/{id}",
            async Task<Results<Ok<NetworkObjectInspectionDateDetails>, BadRequest>> (
                Guid id,
                IQuerySession querySession
                ) =>
            {
                try
                {
                    var networkObject = await querySession
                        .Query<NetworkObjectInspectionDateDetails>()
                        .FirstAsync(n => n.Id == id);
                    return TypedResults.Ok(networkObject);
                }
                catch
                {
                    return TypedResults.BadRequest();
                }                
            });

        netObject.MapGet(
            "/PoleDetails",
            async Task<Results<Ok<IReadOnlyList<PoleDetails>>, BadRequest<string>>> (
                IQuerySession querySession
                ) =>
            {
                try
                {
                    var networkObjects = await querySession
                        .Query<PoleDetails>()                        
                        .ToListAsync(CancellationToken.None);                    

                    return TypedResults.Ok(networkObjects);
                }
                catch(Exception ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
            });
    }
}