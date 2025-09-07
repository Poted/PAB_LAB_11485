using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;

namespace Application.UnitTests.Customers.Commands;

public class RegisterCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    public RegisterCustomerCommandHandlerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmailAlreadyExists()
    {
        var existingEmail = "test@example.com";
        var command = new RegisterCustomerCommand(existingEmail, "Test User", "password", null);

        _mockCustomerRepository.Setup(repo => repo.GetByEmailAsync(existingEmail))
            .ReturnsAsync(new Customer(Guid.NewGuid(), existingEmail, "Existing User", "hashed_pass", "User"));

        var handler = new RegisterCustomerCommandHandler(_mockCustomerRepository.Object, _mockPasswordHasher.Object);

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Customer with this email already exists.");
    }
}