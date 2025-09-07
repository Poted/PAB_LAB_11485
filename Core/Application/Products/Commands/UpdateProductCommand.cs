using MediatR;

namespace Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Sku,
    decimal Price) : IRequest;