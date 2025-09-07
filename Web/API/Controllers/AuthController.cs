using Application.Customers.Commands;
using Application.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCustomerCommand command)
    {
        var customerId = await _mediator.Send(command);
        return Ok(new { CustomerId = customerId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCustomerQuery query)
    {
        var token = await _mediator.Send(query);

        if (token is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(new { Token = token });
    }
}