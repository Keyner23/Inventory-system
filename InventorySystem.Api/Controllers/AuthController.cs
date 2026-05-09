using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Application.Features.Auth;
using Microsoft.AspNetCore.Identity.Data;
using RegisterRequest = InventorySystem.Application.Features.Auth.RegisterRequest;


namespace InventorySystem.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(
        UserManager<AppUser> userManager,
        AppDbContext context,
        IJwtService jwtService
    )
    {
        _userManager = userManager;
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request
    )
    {
        var company = new Company
        {
            Name = request.CompanyName,
            Email = request.Email
        };

        _context.Companies.Add(company);

        await _context.SaveChangesAsync();

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            CompanyId = company.Id
        };

        var result = await _userManager
            .CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            Email = user.Email!
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request
    )
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x =>
                x.Email == request.Email
            );

        if (user == null)
        {
            return Unauthorized();
        }

        var validPassword =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password
            );

        if (!validPassword)
        {
            return Unauthorized();
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            Email = user.Email!
        });
    }
}