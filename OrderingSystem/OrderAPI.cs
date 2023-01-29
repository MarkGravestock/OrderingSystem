using Microsoft.Extensions.DependencyInjection;
using OrderingSystem;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();
builder.Services.AddSingleton<OrderRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");
app.MapPost("/order", (CreateOrderCommand command) => new Order{Id =command.Id});
app.MapPost("/order/create", (CreateOrderCommand command, IMessageBus bus) => bus.InvokeAsync<OrderCreated>(command));

app.Run();

public class CreateOrderHandler
{
    private readonly OrderRepository _repository;

    public CreateOrderHandler(OrderRepository session)
    {
        _repository = session;
    }

    public OrderCreated Handle(CreateOrderCommand command)
    {
        var order = new Order
        {
            Id = command.Id,
            CreatedAt = DateTimeOffset.Now,
            Status = OrderState.Created
        };

        _repository.Store(order);

        return new OrderCreated(order.Id);
    }
}

public class Order
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public OrderState Status { get; set; }
}

public record CreateOrderCommand(Guid Id);

public enum OrderState
{
    Created
}

public class OrderRepository
{
    private Dictionary<Guid, Order> orders = new();

    public void Store(Order order)
    {
        orders[order.Id] = order;
    }

}

public record OrderCreated(Guid id);
