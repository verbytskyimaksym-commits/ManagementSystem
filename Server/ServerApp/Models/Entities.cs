using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class Doctor
{
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class Schedule
{
    public int ID { get; set; }
    public int DoctorID { get; set; }

    [Required]
    public string Date { get; set; } = string.Empty;

    [Required]
    public string TimeSlot { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Available";

    public Doctor? Doctor { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class Appointment
{
    public int ID { get; set; }
    public int DoctorID { get; set; }
    public int? ScheduleID { get; set; }

    [Required]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    public string AppointmentDate { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Pending";

    public Doctor? Doctor { get; set; }
    public Schedule? Schedule { get; set; }
}