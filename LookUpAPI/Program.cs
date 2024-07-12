using Application.Contracts;
using Application.DTOs.TransferDTOs;
using Application.Exceptions;
using LookUpAPI.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<LookUpGetResponseConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<Dictionary<Guid, TaskCompletionSource<LookUpGetResponseDTO>>>();

var app = builder.Build();

app.MapPost("/GetLookUps", async (HttpRequest request, HttpResponse response, IPublishEndpoint _publishEndpoint, Dictionary<Guid, TaskCompletionSource<LookUpGetResponseDTO>> responseTasks) =>
{

    using var httpClient = new HttpClient();

    var correlationId = Guid.NewGuid();

    var tcs = new TaskCompletionSource<LookUpGetResponseDTO>(TaskCreationOptions.RunContinuationsAsynchronously);

    responseTasks.Add(correlationId, tcs); //Adds it to the response Tasks no need for while(true)

    await _publishEndpoint.Publish(new LookUpGetEvent
    {
        CorrelationId = correlationId,
    });

    await response.WriteAsJsonAsync(new InitialResponseDTO
    {
        CorrelationID = correlationId,
        initialResponse = "Request Routed to RabbitMQ"
    });
});

app.MapGet("/GetLookUps", async (HttpRequest request, HttpResponse response, Dictionary<Guid, TaskCompletionSource<LookUpGetResponseDTO>> responseTasks) =>
{

    if (!request.Query.TryGetValue("correlationId", out var correlationIdValue) || !Guid.TryParse(correlationIdValue, out var correlationId))
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        await response.WriteAsJsonAsync("CorrelationId query parameter is missing or invalid.");
        return;
    }

    if (responseTasks.TryGetValue(correlationId, out var tcs))
    {
        if (tcs.Task.IsCompleted && tcs.Task.IsFaulted)
        {
            try
            {
                var result = tcs.Task.Result.Governates; //Just try to access anything and that will throw an exception
            }
            catch (AggregateException aggEx)
            {

                var lookUpsGetExceptions = new List<LookUpsGetException>();

                aggEx.Handle(ex =>
                {
                    if (ex is LookUpsGetException lookUpGetEx)
                    {
                        // Log the error details
                        lookUpsGetExceptions.Add(lookUpGetEx);
                        return true; // Mark this exception as handled
                    }

                    // If it's not handled, return false
                    return false;
                });

                foreach (var lookUpGetException in lookUpsGetExceptions)
                {
                    var errorResponse = new { Message = "Failed to create user", Error = lookUpGetException.Message };
                    await response.WriteAsJsonAsync(lookUpGetException.ErrorModel);
                    responseTasks.Remove(correlationId);
                }
            }
        }
        else if (tcs.Task.IsCompleted)
        {
            var resultGov = tcs.Task.Result.Governates;
            var resultCit = tcs.Task.Result.Cities;
            await response.WriteAsJsonAsync(new
            {
                Governates = resultGov,
                Cities = resultCit
            });
            responseTasks.Remove(tcs.Task.Result.CorrelationId);
        }
        else
        {
            await response.WriteAsJsonAsync("Task is still pending.");
        }
    }
    else
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        await response.WriteAsJsonAsync($"Task with correlationId '{correlationId}' not found. Either Doesn't Exist or already fetched! Who Stole your task");
    }
});



app.Run();