using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/appointments")]
public class AppointmentsApiController : ControllerBase
{
    private readonly AppDbContext _context;
    public AppointmentsApiController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAppointments()
    {
        var query = _context.Appointments.Include(a => a.Doctor).AsQueryable();

        var doctorIdClaim = User.FindFirstValue("DoctorID");
        if (int.TryParse(doctorIdClaim, out int doctorId))
        {
            query = query.Where(a => a.DoctorID == doctorId);
        }

        var result = await query.Select(a => new
        {
            a.ID,
            a.DoctorID,
            DoctorName = a.Doctor != null ? a.Doctor.Name : "",
            a.ScheduleID,
            a.PatientName,
            a.AppointmentDate,
            a.Status
        }).ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var doctorExists = await _context.Doctors.AnyAsync(d => d.ID == request.DoctorID);
        if (!doctorExists)
            return NotFound(new { message = "Лікаря з таким ID не знайдено" });

        var appointment = new Appointment
        {
            DoctorID = request.DoctorID,
            ScheduleID = request.ScheduleID,
            PatientName = request.PatientName,
            AppointmentDate = request.AppointmentDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Confirmed" : request.Status
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointments), new { id = appointment.ID }, new
        {
            appointment.ID,
            appointment.DoctorID,
            appointment.ScheduleID,
            appointment.PatientName,
            appointment.AppointmentDate,
            appointment.Status
        });
    }
}

public class CreateAppointmentRequest
{
    [Required]
    public int DoctorID { get; set; }
    public int? ScheduleID { get; set; }
    [Required]
    public string PatientName { get; set; } = string.Empty;
    [Required]
    public string AppointmentDate { get; set; } = string.Empty;
    public string? Status { get; set; }
}