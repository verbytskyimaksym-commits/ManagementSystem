using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;
    public AppointmentsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Appointments.Include(a => a.Doctor).Include(a => a.Schedule).ToListAsync());

    public IActionResult Create()
    {
        LoadLists();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            LoadLists(appointment.DoctorID, appointment.ScheduleID);
            return View(appointment);
        }
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();
        LoadLists(appointment.DoctorID, appointment.ScheduleID);
        return View(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Appointment appointment)
    {
        if (id != appointment.ID) return BadRequest();
        if (!ModelState.IsValid)
        {
            LoadLists(appointment.DoctorID, appointment.ScheduleID);
            return View(appointment);
        }
        _context.Update(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments.Include(a => a.Doctor).FirstOrDefaultAsync(a => a.ID == id);
        return appointment == null ? NotFound() : View(appointment);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private void LoadLists(int? doctorId = null, int? scheduleId = null)
    {
        ViewBag.Doctors = new SelectList(_context.Doctors, "ID", "Name", doctorId);
        ViewBag.Schedules = new SelectList(
            _context.Schedules.Select(s => new { s.ID, Label = $"{s.Date} ({s.TimeSlot})" }),
            "ID", "Label", scheduleId);
    }
}