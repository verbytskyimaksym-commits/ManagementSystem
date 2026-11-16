using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class DoctorsController : Controller
{
    private readonly AppDbContext _context;
    public DoctorsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Doctors.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        if (!ModelState.IsValid) return View(doctor);
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        return doctor == null ? NotFound() : View(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Doctor doctor)
    {
        if (id != doctor.ID) return BadRequest();
        if (!ModelState.IsValid) return View(doctor);
        _context.Update(doctor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        return doctor == null ? NotFound() : View(doctor);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}