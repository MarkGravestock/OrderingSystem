using Alba;

namespace OrderingSystem;

public class OrderingTests
{
    [Fact]
    public async Task can_call_endpoint()
    {
        await using var host = await AlbaHost.For<Program>();

        await host.Scenario(scenario =>
        {
            scenario.Get.Url("/");
            scenario.ContentShouldBe("Hello World!");
            scenario.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task dummy_creating_an_order_returns_the_new_order()
    {
        await using var host = await AlbaHost.For<Program>();

        var guid = Guid.NewGuid();

        var result = await host.Scenario(scenario =>
        {
            scenario.Post.Json(new CreateOrderCommand(guid)).ToUrl("/order");
            scenario.StatusCodeShouldBeOk();

        });

        var json = await result.ReadAsJsonAsync<Order>();
        Assert.Equal(guid, json.Id);
    }

    [Fact]
    public async Task creating_an_order_returns_the_new_order()
    {
        await using var host = await AlbaHost.For<Program>();

        var guid = Guid.NewGuid();

        var result = await host.Scenario(scenario =>
        {
            scenario.Post.Json(new CreateOrderCommand(guid)).ToUrl("/order/create");
            scenario.StatusCodeShouldBeOk();

        });

        var json = await result.ReadAsJsonAsync<Order>();
        Assert.Equal(guid, json.Id);
    }
}

