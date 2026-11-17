using System.Collections.ObjectModel;
using ClientApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class SchedulesViewModel : ObservableObject
{
    public ObservableCollection<ScheduleItem> Schedules { get; } = new();

    public SchedulesViewModel()
    {
        // Тимчасові тестові дані для перевірки UI (у Завданні 9 замінимо на API)
        Schedules.Add(new ScheduleItem { ID = 1, DoctorID = 1, Date = "2026-11-20", TimeSlot = "09:00 - 10:00", Status = "Available" });
        Schedules.Add(new ScheduleItem { ID = 2, DoctorID = 1, Date = "2026-11-20", TimeSlot = "10:00 - 11:00", Status = "Booked" });
    }

    [RelayCommand]
    private async Task OpenAppointmentAsync(ScheduleItem? schedule)
    {
        if (schedule == null) return;
        await Application.Current!.MainPage!.Navigation.PushAsync(new Views.AppointmentPage(schedule));
    }
}