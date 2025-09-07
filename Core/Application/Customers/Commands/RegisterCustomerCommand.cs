using MediatR;

namespace Application.Customers.Commands;

public record RegisterCustomerCommand(
    string Email,
    string Name,
    string Password,
    string? Role) : IRequest<Guid>;