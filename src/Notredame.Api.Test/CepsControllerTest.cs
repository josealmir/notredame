using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Notredame.App.CreateCep;
using Notredame.Domain.DTOs;
using Notredame.Api.Test.DataTest;
using Notredame.Infra.Data;
using Notredame.Shared.Models;
using System.Text.Json;
using System.Net.Mime;
using System.Text;
using System.Net;

using Notredame.Domain;

using Shouldly;
using Xunit;


namespace Notredame.Api.Test;

public class CepsControllerTest : IClassFixture<ApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ApplicationFactory<Program> _factory;
    private readonly Cep SearchCep; 
    private readonly Cep DeleteCep; 
    public CepsControllerTest(ApplicationFactory<Program> factory)
    {
        _factory = factory;
        var context  = _factory.Services.GetRequiredService<AppDbContext>();
        context.Ceps.AddRange(SetupCepService.CepDtoOk(), SetupCepService.CepDtoOk());
        context.SaveChanges();
        SearchCep = context.Ceps.First();
        DeleteCep = context.Ceps.OrderBy(x=>x.Id).LastOrDefault();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCepPaginated_ShouldReturnOkWithObjectPage()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps");
        
        // Arrange
        var content = JsonSerializer.Deserialize<PageResult<CepDTO>>(await response.Content.ReadAsStringAsync());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        content.ShouldNotBeNull();
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
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStringAsync());
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        problem.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetInCepsWithCepNotFound_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps/00000000");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var problem = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());
        problem.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetInCepsThatHasTimeOut_ShouldReturnTimeOut()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/ceps/99999999");
        
        // Arrange
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.GatewayTimeout);
    }

    [Fact]
    public async Task GetCepByIdNotExiste_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/v1/ceps/{Guid.NewGuid()}");
        
        // Arrange
        var content = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        content.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetCepByIdExiste_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync($"/api/v1/ceps/{SearchCep.ExternalId}");
        
        // Arrange
        var content = JsonSerializer.Deserialize<CepDTO>(await response.Content.ReadAsStringAsync());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        content.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task PostInCepsWithZipCodeInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new CepCommand { ZipCode = "0064" };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        
        // Act
        var response = await _client.PostAsync("/api/v1/ceps", content);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStringAsync());
        problem.ShouldNotBeNull();
    }

    [Fact]
    public async Task PostInCepZipCodeValid_ShouldReturnCreated()
    {
        // Arrange
        var request = new CepCommand { ZipCode = "60872140" };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        
        // Act
        var response = await _client.PostAsync("/api/v1/ceps", content);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = JsonSerializer.Deserialize<CepCreatedDTO>(await response.Content.ReadAsStringAsync());
        result.ShouldNotBeNull();   
    }

    [Fact]
    public async Task DeleteCepNotExist_ShouldReturnUnprocessableEntity()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/v1/ceps/{SetupCepService.CepDtoOk().ExternalId}");
        
        // Arrange
        var content = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        content.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task DeleteCepValid_ShouldReturnOK()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/v1/ceps/{DeleteCep.ExternalId}");
        
        // Arrange
        var content = JsonSerializer.Deserialize<CepDTO>(await response.Content.ReadAsStringAsync());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        content.ShouldNotBeNull();
    }
}
