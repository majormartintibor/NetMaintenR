namespace NetMaintenR.NetReportR;

public static class Endpoints
{
    public static void MapNetReporterEndpoints(this WebApplication app)
    {
        var netObject = app.MapGroup("/netWorker").WithOpenApi();

        netObject.MapGet(
            "/",
            null).WithDisplayName("GetAll");
    }
}