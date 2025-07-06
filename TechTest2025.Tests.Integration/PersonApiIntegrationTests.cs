using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTest2025.Models;

namespace TechTest2025.Tests.Integration;

public class PersonApiIntegrationTests : IClassFixture<WebApplicationFactory<TechTest2025.Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PersonApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    //in real world scenario, you would use testcontainers or a test database for a more accurate integration test setup
    [Fact]
    public async Task GetAllPersons_ReturnsOkAndPersonsList()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/person");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var persons = await response.Content.ReadFromJsonAsync<List<Person>>();
        Assert.NotNull(persons);
        // Optionally, assert on expected data if you control the test data file
    }

}
