using System.Threading;
using System.Threading.Tasks;
using Application.Products.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Application.UnitTests.Products.Commands;

public class AddProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly AddProductCommandHandler _handler;

    public AddProductCommandHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();

        _handler = new AddProductCommandHandler(_mockProductRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_CallAddAsyncOnRepository_WhenCalled()
    {
        var command = new AddProductCommand("Test Product", "TEST-001", 99.99m);

        await _handler.Handle(command, CancellationToken.None);

        _mockProductRepository.Verify(
            repo => repo.AddAsync(It.IsAny<Product>()),
            Times.Once);
    }
}