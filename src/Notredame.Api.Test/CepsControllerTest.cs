using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net;
using Shouldly;
using Xunit;

namespace Notredame.Api.Test;

public class CepsControllerTest : IClassFixture<ApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ApplicationFactory<Program> _factory;

    public CepsControllerTest(ApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetInCepsWithCepValid_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps/60872140");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetInCepsWithCepInValid_ShouldReturnBadRequest()
    {
        // Act
        var response = await _client.GetAsync($"/api/v1/ceps/0064");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStringAsync());
        problem.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetInCepsWithCepNotFound_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps/60874300");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var problem = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());
        problem.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetInCepsWith_ShouldReturnTimeOut()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps/00000000");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.GatewayTimeout);
        var problem = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());
        problem.ShouldNotBeNull();
    }
}
