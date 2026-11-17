using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/schedules")]
public class SchedulesApiController : ControllerBase
{
    private readonly AppDbContext _context;
    public SchedulesApiController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetSchedules()
    {
        var query = _context.Schedules.Include(s => s.Doctor).AsQueryable();

        // Якщо це лікар (з'явиться в Завданні 7), показуємо тільки його графік
        var doctorIdClaim = User.FindFirstValue("DoctorID");
        if (int.TryParse(doctorIdClaim, out int doctorId))
        {
            query = query.Where(s => s.DoctorID == doctorId);
        }

        var result = await query.Select(s => new
        {
            s.ID,
            s.DoctorID,
            DoctorName = s.Doctor != null ? s.Doctor.Name : "",
            s.Date,
            s.TimeSlot,
            s.Status
        }).ToListAsync();

        return Ok(result);
    }
}