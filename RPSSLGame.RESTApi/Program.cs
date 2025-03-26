using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using PSSLGame.Domain.Configuration;
using RPSSLGame.DI;
using RPSSLGame.RESTApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);

builder.Services.Register(builder.Configuration);

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception handling for validation errors
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var response = context.Response;
        var message = exception?.Message ?? "Error";

        var (statusCode, errors, stackTrace) = exception switch
        {
            FluentValidation.ValidationException fluentValidationException => (StatusCodes.Status400BadRequest, fluentValidationException.Errors.Select(e => e.ErrorMessage).ToList(), null),
            UnauthorizedAccessException unauthorizedException => (StatusCodes.Status400BadRequest, new List<string>() { message }, null),
            ArgumentNullException argumentNullException => (StatusCodes.Status400BadRequest, new List<string>() { message }, null),
            ArgumentOutOfRangeException argumentOutOfRangeException => (StatusCodes.Status400BadRequest, new List<string>() { message }, null),
            _ => (StatusCodes.Status500InternalServerError, new List<string>() { message }, exception?.StackTrace)
        };

        context.Response.StatusCode = statusCode;
        if (stackTrace != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = errors,
                StackTrace = stackTrace
            });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = errors
            });
        }
    });
});

app.RegisterEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.Run();

public partial class Program { }


