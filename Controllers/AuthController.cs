using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using TeamHeartFiap.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TeamHeartFiap.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtConfig _jwt;
    public AuthController(IOptions<JwtConfig> jwtOptions) => _jwt = jwtOptions.Value;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, req.Username ?? "usuario"),
            new Claim(ClaimTypes.Role, req.Role ?? "User"),
            new Claim("nome", req.Username ?? "usuario")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Role { get; set; } // exemplo: "Admin" ou "User"
}