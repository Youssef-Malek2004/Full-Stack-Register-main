using System.Reflection;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.CommandHandlers;
using MassTransit;
using api.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); ;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());
//Check out https://stackoverflow.com/questions/75848218/no-service-for-type-mediatr-irequesthandler-has-been-registred-net-6 to explain why i did this future youssef


builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<UserCreateEventConsumer>();

    busConfigurator.AddConsumer<LookUpGetEventConsumer>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

