using Marten;
using NetMaintenR.NetObject;
using Weasel.Core;
using static NetMaintenR.NetObject.NetworkObjectCommand;

namespace NetMaintenR.Tests.NetObject;
public class CreateNetObjectTests
{
    private readonly DocumentStore theStore;

    public CreateNetObjectTests()
    { 
        theStore = DocumentStore.For(options =>
        {
            options.Connection(TestData.ConnectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;

            options.AddNetworkObjectProjections();
        });        
    }

    [Fact]
    public async Task CreatePole_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreatePole(id), CancellationToken.None);


        var pole = await session.Events.AggregateStreamAsync<NetworkObject>(id);        
        Assert.NotNull(pole);
        Assert.Equal(id, pole.Id);


        var isPole = pole is NetworkObject.Pole;
        Assert.True(isPole);


        var networkObjectInspectionDateDetails = await session
                        .Query<NetworkObjectInspectionDateDetails>()
                        .SingleAsync(n => n.Id == id);        
        Assert.Equal(id, networkObjectInspectionDateDetails.Id);


        var poleDetails = await session
                        .Query<PoleDetails>()
                        .SingleAsync(n => n.Id == id);
        Assert.Equal(id, poleDetails.Id);


        await theStore.Advanced.ResetAllData();        
    }

    [Fact]
    public async Task CreateSection_should_succeed()
    {
        using var session = theStore.LightweightSession();
        var id = Guid.NewGuid();
        await session.Decide(id, new CreateSection(id), CancellationToken.None);


        var section = await session.Events.AggregateStreamAsync<NetworkObject>(id);
        Assert.NotNull(section);
        Assert.Equal(id, section.Id);


        var isSection = section is NetworkObject.Section;
        Assert.True(isSection);


        var networkObjectInspectionDateDetails = await session
                        .Query<NetworkObjectInspectionDateDetails>()
                        .SingleAsync(n => n.Id == id);        
        Assert.Equal(id, networkObjectInspectionDateDetails.Id);


        var sectionDetails = await session
                        .Query<NetworkObjectInspectionDateDetails>()
                        .SingleAsync(n => n.Id == id);
        Assert.Equal(id, sectionDetails.Id);


        await theStore.Advanced.ResetAllData();        
    }
}
