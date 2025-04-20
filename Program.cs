using System.Text.Json;
using System.Text.Json;
using BuffBuddyAPI;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var azureString = builder.Configuration["AZURE_STORAGE_CONNECTION_STRING"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});

var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(",");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("total-count")
    );
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection")
);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IFileStorage, AzureFileStorage>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = exception?.Message,
            stackTrace = exception?.StackTrace
        }));
    });
});




app.UseHttpsRedirection();

app.UseCors();

app.UseStaticFiles();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
