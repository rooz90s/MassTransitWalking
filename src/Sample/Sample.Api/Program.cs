using MassTransit;
using Sample.Component.Consumers;
using Sample.Component.StateMachines;
using Sample.Component.StateMachines.StateMachineActivities;
using Sample.Component.StateMachines.States;
using Sample.Contract;
using Sample.Contract.SagaEvents;
using Warehouse.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(options =>
{

    options.AddActivitiesFromNamespaceContaining<AcceptOrderActivity>();

    options.AddConsumer<SubmitOrderConsumer>();

    options.UsingRabbitMq((ctx, conf) =>
    {
        conf.Host("amqp://guest:guest@localhost:33001");
        conf.UseMessageRetry(r => r.Immediate(2));
        conf.ConfigureEndpoints(ctx);
    });

    options.AddSagaStateMachine<OrderStateMachine, OrderState>(typeof(OrderSateMachineDefinition))
        .MongoDbRepository(r =>
        {
            r.Connection = "mongodb://127.0.0.1:33002";
            r.CollectionName = "OrderSaga";
            r.DatabaseName = "MasstransitOrder";
        });


    options.AddRequestClient<ISubmitOrder>();
    options.AddRequestClient<IOrderCheck>();
    options.AddRequestClient<IAllocateInventory>();
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
