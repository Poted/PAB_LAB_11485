using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDto?>;