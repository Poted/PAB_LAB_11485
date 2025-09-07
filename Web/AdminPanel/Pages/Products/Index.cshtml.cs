using Microsoft.AspNetCore.Mvc.RazorPages;
using StockService;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace AdminPanel.Pages.Products;

public class ProductDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("sku")]
    public string Sku { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}


public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Stock.StockClient _stockClient;

    public IndexModel(IHttpClientFactory httpClientFactory, Stock.StockClient stockClient)
    {
        _httpClientFactory = httpClientFactory;
        _stockClient = stockClient;
    }

    public List<ProductDto> Products { get; set; } = new();

    public Dictionary<Guid, int> StockLevels { get; set; } = new();

    public async Task OnGetAsync()
    {
        var token = Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync("api/products");

        if (response.IsSuccessStatusCode)
        {
            Products = await response.Content.ReadFromJsonAsync<List<ProductDto>>() ?? new List<ProductDto>();

            foreach (var product in Products)
            {
                var stockRequest = new GetStockRequest { ProductId = product.Id.ToString() };
                var stockReply = await _stockClient.GetStockAsync(stockRequest);
                StockLevels[product.Id] = stockReply.QuantityOnHand;
            }
        }
    }
}

