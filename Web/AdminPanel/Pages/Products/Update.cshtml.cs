using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace AdminPanel.Pages.Products;

public class UpdateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UpdateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public ProductDto Product { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var token = Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Product = await httpClient.GetFromJsonAsync<ProductDto>($"api/products/{id}");

        if (Product == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var token = Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            Name = Product.Name,
            Sku = Product.Sku,
            Price = Product.Price
        };

        var response = await httpClient.PutAsJsonAsync($"api/products/{Product.Id}", payload);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        return Page();
    }
}