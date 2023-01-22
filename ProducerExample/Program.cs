using Dapr.Client;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();

var app = builder.Build();

app.MapGet("/api/send/{message}", async (DaprClient daprClient, string message) =>
{
    var pubSub = "servicebus-pubsub";
    var topicName = "avalar";

    try
    {
        await daprClient.PublishEventAsync(
            pubSub,
            topicName,
            JsonConvert.SerializeObject(message));
    }
    catch (Exception exception)
    {
        return Results.BadRequest($"{exception.Message}: {exception.InnerException!.Message}");
    }

    return Results.Ok("Message sent");
});

app.Run();