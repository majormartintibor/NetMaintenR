namespace NetMaintenR.NetObject;

public static class Endpoints
{
    public static void MapNetObjectEndpoints(this WebApplication app)
    {
        var netObject = app.MapGroup("/netObject").WithOpenApi();

        netObject.MapPost(
            "/",
            null).WithDisplayName("CreateNew");
    }
}