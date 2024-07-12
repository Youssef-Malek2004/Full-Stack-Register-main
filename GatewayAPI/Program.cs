using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using Application.Contracts;
using Application.DTOs.TransferDTOs;
using Application.DTOs.UserDTOs;
using Application.Exceptions;
using GatewayAPI.Consumers;
using MassTransit;
using MassTransit.Internals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<UserCreateResponseConsumer>();

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

builder.Services.AddSingleton<UserCreateResponseConsumer>();
builder.Services.AddSingleton<Dictionary<Guid, TaskCompletionSource<TCSDTO>>>();

var app = builder.Build();

app.MapPost("/register", async (HttpRequest request, HttpResponse response, IPublishEndpoint _publishEndpoint, UserCreateResponseConsumer responseConsumer, Dictionary<Guid, TaskCompletionSource<TCSDTO>> responseTasks) =>
{
    using var httpClient = new HttpClient();

    // Read the incoming JSON request body
    var incomingData = await request.ReadFromJsonAsync<CreateUserRequestDTO>();

    var correlationId = Guid.NewGuid();

    //Response Should be the task CorrelationID, so that react can get it back

    var tcs = new TaskCompletionSource<TCSDTO>(TaskCreationOptions.RunContinuationsAsynchronously);

    responseTasks.Add(correlationId, tcs); //Adds it to the response Tasks no need for while(true)

    await _publishEndpoint.Publish(new UserCreateEvent
    {
        CorrelationId = correlationId,
        UserModelDTO = incomingData
    });

    await response.WriteAsJsonAsync(new InitialResponseDTO
    {
        CorrelationID = correlationId,
        initialResponse = "Request Routed to RabbitMQ"
    });
});

app.MapGet("/register", async (HttpRequest request, HttpResponse response, Dictionary<Guid, TaskCompletionSource<TCSDTO>> responseTasks) =>
{
    if (!request.Query.TryGetValue("correlationId", out var correlationIdValue) || !Guid.TryParse(correlationIdValue, out var correlationId))
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        await response.WriteAsync("CorrelationId query parameter is missing or invalid.");
        return;
    }

    if (responseTasks.TryGetValue(correlationId, out var tcs))
    {
        if (tcs.Task.IsCompleted && tcs.Task.IsFaulted)
        {
            try
            {
                var result = tcs.Task.Result.ErrorResponseDTO;
            }
            catch (AggregateException aggEx)
            {

                var userCreationExceptions = new List<UserCreationException>();

                aggEx.Handle(ex =>
                {
                    if (ex is UserCreationException userCreationEx)
                    {
                        // Log the error details
                        userCreationExceptions.Add(userCreationEx);
                        return true; // Mark this exception as handled
                    }

                    // If it's not handled, return false
                    return false;
                });

                foreach (var userCreationEx in userCreationExceptions)
                {
                    var errorResponse = new { Message = "Failed to create user", Error = userCreationEx.Message };
                    await response.WriteAsJsonAsync(userCreationEx.ErrorModel);
                    responseTasks.Remove(correlationId);
                }
            }
        }
        else if (tcs.Task.IsCompleted)
        {
            var result = tcs.Task.Result.UserDTO;
            await response.WriteAsJsonAsync(result);
            responseTasks.Remove(tcs.Task.Result.CorrelationID);
        }
        else
        {
            await response.WriteAsync("Task is still pending.");
        }
    }
    else
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        await response.WriteAsync($"Task with correlationId '{correlationId}' not found.");
    }
});
app.UseCors("AllowAll");

app.Run();

// Models for deserializing JSON