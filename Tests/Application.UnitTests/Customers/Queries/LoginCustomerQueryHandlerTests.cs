using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Queries;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;

namespace Application.UnitTests.Customers.Queries;

public class LoginCustomerQueryHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtProvider> _mockJwtProvider;
    private readonly LoginCustomerQueryHandler _handler;

    public LoginCustomerQueryHandlerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtProvider = new Mock<IJwtProvider>();
        _handler = new LoginCustomerQueryHandler(
            _mockCustomerRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtProvider.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnToken_WhenCredentialsAreValid()
    {
        var userEmail = "test@example.com";
        var userPassword = "password123";
        var hashedPassword = "hashed_password";
        var expectedToken = "test_jwt_token";

        var customer = new Customer(Guid.NewGuid(), userEmail, "Test User", hashedPassword, "User");
        var query = new LoginCustomerQuery(userEmail, userPassword);

        _mockCustomerRepository.Setup(repo => repo.GetByEmailAsync(userEmail)).ReturnsAsync(customer);
        _mockPasswordHasher.Setup(hasher => hasher.HashPassword(userPassword)).Returns(hashedPassword);
        _mockJwtProvider.Setup(provider => provider.GenerateToken(customer)).Returns(expectedToken);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().Be(expectedToken);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenPasswordIsInvalid()
    {
        var userEmail = "test@example.com";
        var correctPassword = "password123";
        var incorrectPassword = "wrong_password";

        var customer = new Customer(Guid.NewGuid(), userEmail, "Test User", "hashed_correct_password", "User");
        var query = new LoginCustomerQuery(userEmail, incorrectPassword);

        _mockCustomerRepository.Setup(repo => repo.GetByEmailAsync(userEmail)).ReturnsAsync(customer);

        _mockPasswordHasher.Setup(hasher => hasher.HashPassword(correctPassword)).Returns("hashed_correct_password");
        _mockPasswordHasher.Setup(hasher => hasher.HashPassword(incorrectPassword)).Returns("hashed_wrong_password");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}