using Marten;
using NetMaintenR.NetCloseR;
using NetMaintenR.NetInspectR;
using NetMaintenR.NetJobR;
using NetMaintenR.NetObject;
using NetMaintenR.NetReportR;
using NetMaintenR.NetWorkR;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMarten(sp =>
{
    StoreOptions options = new();

    options.Connection(builder.Configuration.GetConnectionString("Default")!);

    options.UseDefaultSerialization(EnumStorage.AsInteger);

    if(builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }

    options.AddNetworkObjectProjections();

    return options;
}).UseLightweightSessions();

var app = builder.Build();

app.MapNetCloserEndpoints();
app.MapNetInspectorEndpoints();
app.MapNetJobEndpoints();
app.MapNetObjectEndpoints();
app.MapNetReporterEndpoints();
app.MapNetWorkerEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();