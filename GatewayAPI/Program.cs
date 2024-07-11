using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using Application.Contracts;
using Application.DTOs.UserDTOs;
using GatewayAPI.Consumers;
using MassTransit;
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

var app = builder.Build();

app.MapPost("/register", async (HttpRequest request, HttpResponse response, IPublishEndpoint _publishEndpoint, UserCreateResponseConsumer responseConsumer) =>
{
    using var httpClient = new HttpClient();

    // Read the incoming JSON request body
    var incomingData = await request.ReadFromJsonAsync<CreateUserRequestDTO>();

    var correlationId = Guid.NewGuid();

    await _publishEndpoint.Publish(new UserCreateEvent
    {
        CorrelationId = correlationId,
        UserModelDTO = incomingData
    });

    try
    {
        // Wait for the response with the matching correlation ID
        var responseData = await responseConsumer.WaitForResponse(correlationId);


        // Return the response data to the client
        await response.WriteAsJsonAsync(responseData);
    }
    catch (Exception ex)
    {
        // Handle any errors
        var errorResponse = new { Message = "Failed to create user", Error = ex.Message };
        await response.WriteAsJsonAsync(errorResponse);
    }
});

app.UseCors("AllowAll");

app.Run();

// Models for deserializing JSON