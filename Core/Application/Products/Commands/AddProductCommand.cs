using MediatR;

namespace Application.Products.Commands;

public record AddProductCommand(
    string Name,
    string Sku,
    decimal Price) : IRequest<Guid>;