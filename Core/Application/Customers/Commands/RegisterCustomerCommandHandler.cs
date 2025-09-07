using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Customers.Commands;

public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IPasswordHasher passwordHasher)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(request.Email);
        if (existingCustomer is not null)
        {
            throw new Exception("Customer with this email already exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var role = request.Role == "Admin" ? "Admin" : "User";

        var customer = new Customer(
            Guid.NewGuid(),
            request.Email,
            request.Name,
            hashedPassword,
            role
        );

        await _customerRepository.AddAsync(customer);

        return customer.Id;
    }
}