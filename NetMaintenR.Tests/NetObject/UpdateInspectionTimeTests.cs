using Marten;
using NetMaintenR.NetObject;
using Weasel.Core;
using static NetMaintenR.NetObject.NetworkObjectCommand;

namespace NetMaintenR.Tests.NetObject;
public class UpdateInspectionTimeTests
{    
    private DocumentStore theStore;

    public UpdateInspectionTimeTests()
    {  
        theStore = DocumentStore.For(options =>
        {
            options.Connection(TestData.ConnectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;

            options.AddNetworkObjectProjections();
        });
    }


    [Fact]
    public async Task Pole_UpdateLastMinorInspection_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreatePole(id), CancellationToken.None);
        

        var pole = await session.Events.AggregateStreamAsync<NetworkObject>(id);
        Assert.NotNull(pole);
        var date = DateTimeOffset.Now;
        await session.Decide(pole.Id, new UpdateLastMinorInspection(date), CancellationToken.None);


        var poleDetails = await session
                .Query<NetworkObjectInspectionDateDetails>()
                .SingleAsync(p => p.Id == pole.Id);
        Assert.Equal(date, poleDetails.LastMinorInspection);


        await theStore.Advanced.ResetAllData();       
    }

    [Fact]
    public async Task Pole_UpdateLastMajorInspection_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreatePole(id), CancellationToken.None);


        var pole = await session.Events.AggregateStreamAsync<NetworkObject>(id);
        Assert.NotNull(pole);
        var date = DateTimeOffset.Now;
        await session.Decide(pole.Id, new UpdateLastMajorInspection(date), CancellationToken.None);


        var poleDetails = await session
                .Query<NetworkObjectInspectionDateDetails>()
                .SingleAsync(p => p.Id == pole.Id);
        Assert.Equal(date, poleDetails.LastMajorInspection);


        await theStore.Advanced.ResetAllData();
    }

    [Fact]
    public async Task Section_UpdateLastMinorInspection_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreateSection(id), CancellationToken.None);


        var section = await session.Events.AggregateStreamAsync<NetworkObject>(id);
        Assert.NotNull(section);
        var date = DateTimeOffset.Now;
        await session.Decide(section.Id, new UpdateLastMinorInspection(date), CancellationToken.None);


        var poleDetails = await session
                .Query<NetworkObjectInspectionDateDetails>()
                .SingleAsync(p => p.Id == section.Id);
        Assert.Equal(date, poleDetails.LastMinorInspection);


        await theStore.Advanced.ResetAllData();
    }

    [Fact]
    public async Task Section_UpdateLastMajorInspection_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreateSection(id), CancellationToken.None);


        var section = await session.Events.AggregateStreamAsync<NetworkObject>(id);
        Assert.NotNull(section);
        var date = DateTimeOffset.Now;
        await session.Decide(section.Id, new UpdateLastMajorInspection(date), CancellationToken.None);


        var poleDetails = await session
                .Query<NetworkObjectInspectionDateDetails>()
                .SingleAsync(p => p.Id == section.Id);
        Assert.Equal(date, poleDetails.LastMajorInspection);


        await theStore.Advanced.ResetAllData();
    }
}