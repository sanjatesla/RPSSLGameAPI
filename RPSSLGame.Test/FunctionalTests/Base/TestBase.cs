using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace RPSSLGame.Test.FunctionalTests.Base;

[TestFixture]
public class TestBase
{
    protected HttpClient Client { get; private set; }
    private WebApplicationFactory<Program> _factory;
    protected JsonSerializerOptions _deserializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true, // Ignore case when matching property names
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camelCase naming policy
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignore null values when writing JSON
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Handle enum values as strings
    };
    protected StringContent? GetStringContent<T>(T command) 
    {
        return new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
    }

    protected async Task<T> ConvertResponse<T>(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseString, _deserializerOptions);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        Client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Client.Dispose();
        _factory.Dispose();
    }
}