using Alba;

namespace OrderingSystem;

public class OrderingTests
{
    [Fact]
    public async Task can_call_endpoint()
    {
        await using var host = await AlbaHost.For<Program>();

        // This runs an HTTP request and makes an assertion
        // about the expected content of the response
        await host.Scenario(_ =>
        {
            _.Get.Url("/");
            _.ContentShouldBe("Hello World!");
            _.StatusCodeShouldBeOk();
        });
    }
}