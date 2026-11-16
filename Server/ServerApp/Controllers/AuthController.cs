using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ServerApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    // Логін та пароль адміністратора у коді (за методичкою)
    private const string AdminLogin = "admin";
    private const string AdminPassword = "admin123";

    private const string JwtKey = "SuperSecretKeyForMedicalSystem2026!!";
    private const string JwtIssuer = "MedicalSystemServer";
    private const string JwtAudience = "MedicalSystemUsers";

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public IActionResult Login([FromForm] string username, [FromForm] string password)
    {
        if (username != AdminLogin || password != AdminPassword)
        {
            ViewBag.Error = "Невірний логін або пароль";
            return View();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        Response.Cookies.Append("JwtToken", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddHours(2)
        });

        return Redirect("/auth/dashboard");
    }

    [Authorize]
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return Content($"Вітаємо, {User.Identity?.Name}! Доступ до адмін-панелі дозволено (JWT працює).", "text/plain; charset=utf-8");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("JwtToken");
        return Redirect("/auth/login");
    }
}