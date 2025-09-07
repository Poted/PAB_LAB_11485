using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace API.EndToEnd.Tests;

public class LoginResponse { public string? Token { get; set; } }

public class ApiFlowTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiFlowTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullUserFlow_WithCreateEditAndDelete_ShouldSucceed()
    {
        // 1: Register
        var uniqueEmail = $"admin.e2e.{System.Guid.NewGuid()}@example.com";
        var registerData = new { email = uniqueEmail, name = "Admin E2E", password = "Password123", role = "Admin" };
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerData);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 2: Login
        var loginData = new { email = uniqueEmail, password = "Password123" };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginData);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        loginResult.Should().NotBeNull();
        loginResult!.Token.Should().NotBeNullOrEmpty();
        var token = loginResult.Token;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 3: Add Product
        var productData = new { name = "Produkt Testowy E2E", sku = "E2E-001", price = 199.99 };
        var createProductResponse = await _client.PostAsJsonAsync("/api/products", productData);
        createProductResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var locationUri = createProductResponse.Headers.Location;
        locationUri.Should().NotBeNull();
        var newProductId = locationUri.Segments.Last();

        // 4: Update Product
        var updatedProductData = new { name = "Zaktualizowany Produkt", sku = "E2E-001-UPDATED", price = 250.00 };
        var updateResponse = await _client.PutAsJsonAsync($"/api/products/{newProductId}", updatedProductData);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 5: Delete Product
        var deleteResponse = await _client.DeleteAsync($"/api/products/{newProductId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}