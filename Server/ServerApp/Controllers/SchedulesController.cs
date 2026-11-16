using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class SchedulesController : Controller
{
    private readonly AppDbContext _context;
    public SchedulesController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Schedules.Include(s => s.Doctor).ToListAsync());

    public IActionResult Create()
    {
        ViewBag.Doctors = new SelectList(_context.Doctors, "ID", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Schedule schedule)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "ID", "Name", schedule.DoctorID);
            return View(schedule);
        }
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null) return NotFound();
        ViewBag.Doctors = new SelectList(_context.Doctors, "ID", "Name", schedule.DoctorID);
        return View(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Schedule schedule)
    {
        if (id != schedule.ID) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "ID", "Name", schedule.DoctorID);
            return View(schedule);
        }
        _context.Update(schedule);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var schedule = await _context.Schedules.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.ID == id);
        return schedule == null ? NotFound() : View(schedule);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}