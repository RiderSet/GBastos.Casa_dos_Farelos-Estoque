using GBastos.Casa_dos_farelos_Estoque.Api.Application.Handlers;
using GBastos.Casa_dos_farelos_Estoque.Api.Endpoints;
using GBastos.Casa_dos_farelos_Estoque.Api.Endpoints.V1;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Messaging;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_farelos_Estoque.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]!));

builder.Services.AddScoped<VendaCriadaHandler>();
builder.Services.AddHostedService<VendaCriadaConsumer>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!)
    .AddRedis(builder.Configuration["Redis:Connection"]!);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Estoque Service API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealth();
app.MapEstoqueV1();

app.Run();