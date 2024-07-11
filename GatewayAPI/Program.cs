using System.Net.Http;
using System.Net.Http.Json;
using Application.DTOs.UserDTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/register", async (HttpRequest request, HttpResponse response) =>
{
    using var httpClient = new HttpClient();

    // Read the incoming JSON request body
    var incomingData = await request.ReadFromJsonAsync<CreateUserRequestDTO>();

    // Post the data to the target API
    var targetResponse = await httpClient.PostAsJsonAsync("http://localhost:5144/api/user", incomingData);

    response.StatusCode = (int)targetResponse.StatusCode;

    if (targetResponse.IsSuccessStatusCode)
    {
        // Read the response content from the target API
        var responseData = await targetResponse.Content.ReadFromJsonAsync<UserDTO>();

        // Return the response content back to the frontend
        await response.WriteAsJsonAsync(responseData);
    }
    else
    {
        // Read the error response content from the target API
        var errorResponse = await targetResponse.Content.ReadFromJsonAsync<errorResponse>();

        // Return the error response content back to the frontend
        await response.WriteAsJsonAsync(errorResponse);
    }
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

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

app.Run();

// Models for deserializing JSON
class errorResponse
{
    public string Message { get; set; } = String.Empty;
    public string Errors { get; set; } = String.Empty;
}