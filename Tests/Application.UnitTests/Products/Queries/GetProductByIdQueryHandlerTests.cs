using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Dtos;
using Application.Products.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;

namespace Application.UnitTests.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;

    public GetProductByIdQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task Handle_Should_ReturnProductDto_WhenProductExists()
    {
        var productId = Guid.NewGuid();
        var product = new Product(productId, "Test Product", "TEST-001", 100m);
        var query = new GetProductByIdQuery(productId);

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var handler = new GetProductByIdQueryHandler(_mockProductRepository.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<ProductDto>();
        result.Id.Should().Be(productId);
        result.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        var query = new GetProductByIdQuery(productId);

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync((Product)null);

        var handler = new GetProductByIdQueryHandler(_mockProductRepository.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}