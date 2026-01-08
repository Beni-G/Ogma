using Ogma.Api.Middleware;
using Ogma.Application.Extensions;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using Ogma.Infrastructure.Persistence.Orders.Extensions;
using Ogma.Infrastructure.Persistence.Partners.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCatalogPersistence(builder.Configuration);
builder.Services.AddPartnersPersistence(builder.Configuration);
builder.Services.AddOrdersPersistence(builder.Configuration);

builder.Services.AddApplicationLayer();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Ogma")
        .WithTheme(ScalarTheme.Saturn)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
