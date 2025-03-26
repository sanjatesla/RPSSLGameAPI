using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using PSSLGame.Domain.Configuration;
using RPSSLGame.DI;
using RPSSLGame.RESTApi;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Configure AppSettings from appsettings.json and register as singleton
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);

// Register services
builder.Services.Register(builder.Configuration);

// Add configuration
builder.Configuration
    .AddEnvironmentVariables();

// Configure JSON options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Add Swagger
// Register the Swagger generator, defining 1 or more Swagger documents
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

        // Default to a 500 Internal Server Error Status Code, unless there's a more specific status code
        var (statusCode, errors, stackTrace) = exception switch
        {
            FluentValidation.ValidationException fluentValidationException => (StatusCodes.Status400BadRequest, fluentValidationException.Errors.Select(e => e.ErrorMessage).ToList(), null),
            UnauthorizedAccessException unauthorizedException => (StatusCodes.Status401Unauthorized, new List<string>() { message }, null),
            ArgumentNullException argumentNullException => (StatusCodes.Status400BadRequest, new List<string>() { message }, null),
            ArgumentOutOfRangeException argumentOutOfRangeException => (StatusCodes.Status400BadRequest, new List<string>() { message }, null),
            _ => (StatusCodes.Status500InternalServerError, new List<string>() { message }, exception?.StackTrace)
        };

        context.Response.StatusCode = statusCode;
        if (stackTrace != null)
        {
            // for development, show the stack trace if status code is 500
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

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
app.UseSwaggerUI();

// Enable middleware to serve static files
app.UseRouting();
app.Run();

public partial class Program { }


