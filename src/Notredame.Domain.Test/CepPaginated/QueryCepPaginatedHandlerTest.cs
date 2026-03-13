using MapsterMapper;
using Microsoft.Extensions.Logging;
using Notredame.App.CepPaginated;
using Notredame.Domain.DTOs;
using Notredame.Domain.Repositories;
using Notredame.Shared.Models;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test.CepPaginated;

public sealed class QueryCepPaginatedHandlerTest
{
    private readonly ICepRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<QueryCepPaginatedHandler> _logger;
    private readonly QueryCepPaginatedHandler _handler;

    public QueryCepPaginatedHandlerTest()
    {
        _repository = Substitute.For<ICepRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<QueryCepPaginatedHandler>>();

        _handler = new QueryCepPaginatedHandler(_logger, _repository, _mapper);
    }

    [Fact]
    public async Task WhenDataExists_ShouldReturnPaginatedResult()
    {
        // Arrange
        var query = new QueryCepPaginated
        {
            PageNumber = 1,
            PageSize = 10,
            SearchBy = "Fortaleza"
        };

        var ceps = new List<Cep>
        {
            new()
            {
                ZipCode = "60872140",
                City = "Fortaleza",
                District = "Messejana",
                State = "CE",
                Provider = ProviderCep.Brazilapi,
                Location = new Location { Lat = -3.8, Lon = -38.5 }
            }
        };

        var pageResult = new PageResult<Cep>(ceps, 1, 1, 10);

        _repository
            .GetAllPaginatedAsync(query.PageNumber, query.PageSize, query.SearchBy)
            .Returns(pageResult);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Page.ShouldNotBeNull();
        result.Value.Page.TotalData.ShouldBe(1);
        result.Value.Page.Current.ShouldBe(1);
        result.Value.Page.Size.ShouldBe(10);
    }

    [Fact]
    public async Task WhenNoData_ShouldReturnEmptyPaginatedResult()
    {
        // Arrange
        var query = new QueryCepPaginated
        {
            PageNumber = 1,
            PageSize = 10,
            SearchBy = "Inexistente"
        };

        var pageResult = new PageResult<Cep>(Enumerable.Empty<Cep>(), 0, 1, 10);

        _repository
            .GetAllPaginatedAsync(query.PageNumber, query.PageSize, query.SearchBy)
            .Returns(pageResult);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Data.ShouldBeEmpty();
        result.Value.Page.TotalData.ShouldBe(0);
    }

    [Fact]
    public async Task ShouldUseDefaultPaginationValues()
    {
        // Arrange
        var query = new QueryCepPaginated();

        var pageResult = new PageResult<Cep>(Enumerable.Empty<Cep>(), 0, 1, 10);

        _repository
            .GetAllPaginatedAsync(1, 10, null)
            .Returns(pageResult);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        query.PageNumber.ShouldBe(1);
        query.PageSize.ShouldBe(10);
        query.SearchBy.ShouldBeNull();
    }
}
