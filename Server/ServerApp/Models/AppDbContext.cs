using Microsoft.EntityFrameworkCore;

namespace ServerApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>()
            .HasIndex(d => d.Email)
            .IsUnique();

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Doctor)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.DoctorID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Schedule)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ScheduleID)
            .OnDelete(DeleteBehavior.SetNull);

        // Тестові дані (щоб БД створювалася одразу заповненою)
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { ID = 1, Name = "Др. Олена Коваленко", Email = "kovalenko@clinic.ua", PasswordHash = "hash_doctor_1" },
            new Doctor { ID = 2, Name = "Др. Андрій Мельник", Email = "melnyk@clinic.ua", PasswordHash = "hash_doctor_2" },
            new Doctor { ID = 3, Name = "Др. Ірина Бондар", Email = "bondar@clinic.ua", PasswordHash = "hash_doctor_3" }
        );

        modelBuilder.Entity<Schedule>().HasData(
            new Schedule { ID = 1, DoctorID = 1, Date = "2026-11-20", TimeSlot = "09:00 - 10:00", Status = "Available" },
            new Schedule { ID = 2, DoctorID = 1, Date = "2026-11-20", TimeSlot = "10:00 - 11:00", Status = "Booked" },
            new Schedule { ID = 3, DoctorID = 2, Date = "2026-11-21", TimeSlot = "12:00 - 13:00", Status = "Available" }
        );

        modelBuilder.Entity<Appointment>().HasData(
            new Appointment { ID = 1, DoctorID = 1, ScheduleID = 2, PatientName = "Іван Петренко", AppointmentDate = "2026-11-20 10:00", Status = "Confirmed" },
            new Appointment { ID = 2, DoctorID = 1, ScheduleID = 1, PatientName = "Марія Сидоренко", AppointmentDate = "2026-11-20 09:00", Status = "Pending" },
            new Appointment { ID = 3, DoctorID = 2, ScheduleID = 3, PatientName = "Олег Ткаченко", AppointmentDate = "2026-11-21 12:00", Status = "Pending" }
        );
    }
}