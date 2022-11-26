using Api.DatabaseConfiguration;
using Api.Extensions;
using Api.Queries;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using InfrastructureLayer.Consumers;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.SetupDatabase();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, InfrastructureLayer.Handlers.EventHandler>(x => new InfrastructureLayer.Handlers.EventHandler(builder.Services));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.RegisterQueryHandler();

builder.Services.AddControllers();
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
