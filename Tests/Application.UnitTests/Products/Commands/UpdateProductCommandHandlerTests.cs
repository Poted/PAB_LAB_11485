using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;

namespace Application.UnitTests.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;

    public UpdateProductCommandHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenProductNotFound()
    {
        var nonExistentProductId = Guid.NewGuid();
        var command = new UpdateProductCommand(nonExistentProductId, "New Name", "NEW-SKU", 1.0m);

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(nonExistentProductId))
            .ReturnsAsync((Product)null);

        var handler = new UpdateProductCommandHandler(_mockProductRepository.Object);

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("Product not found.");
    }
}