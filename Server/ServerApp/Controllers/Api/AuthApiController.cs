using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;

namespace ServerApp.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly AppDbContext _context;

    private const string JwtKey = "SuperSecretKeyForMedicalSystem2026!!";
    private const string JwtIssuer = "MedicalSystemServer";
    private const string JwtAudience = "MedicalSystemUsers";

    public AuthApiController(AppDbContext context) => _context = context;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DoctorLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == request.Email);
        if (doctor == null)
            return Unauthorized(new { message = "Невірний email або пароль" });

        // Підтримка і BCrypt-хешу, і початкових тестових записів із бази
        bool isValidPassword = doctor.PasswordHash.StartsWith("$2")
            ? BCrypt.Net.BCrypt.Verify(request.Password, doctor.PasswordHash)
            : doctor.PasswordHash == request.Password;

        if (!isValidPassword)
            return Unauthorized(new { message = "Невірний email або пароль" });

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, doctor.Name),
            new Claim(ClaimTypes.Email, doctor.Email),
            new Claim(ClaimTypes.Role, "Doctor"),
            new Claim("DoctorID", doctor.ID.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            doctorId = doctor.ID,
            name = doctor.Name,
            email = doctor.Email
        });
    }
}

public class DoctorLoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}