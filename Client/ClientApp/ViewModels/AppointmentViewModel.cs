using ClientApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class AppointmentViewModel : ObservableObject
{
    [ObservableProperty] private int doctorId;
    [ObservableProperty] private int scheduleId;
    [ObservableProperty] private string scheduleInfo = string.Empty;
    [ObservableProperty] private string patientName = string.Empty;
    [ObservableProperty] private string appointmentDate = string.Empty;
    [ObservableProperty] private string statusMessage = string.Empty;

    public AppointmentViewModel(ScheduleItem schedule)
    {
        DoctorId = schedule.DoctorID;
        ScheduleId = schedule.ID;
        ScheduleInfo = $"{schedule.Date} ({schedule.TimeSlot})";
        AppointmentDate = $"{schedule.Date} {schedule.TimeSlot.Split(' ')[0]}";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(PatientName))
        {
            StatusMessage = "Введіть ім'я пацієнта!";
            return;
        }

        StatusMessage = $"Запис для {PatientName} підготовлено (локальний тест).";
        await Task.Delay(500);
        await Application.Current!.MainPage!.Navigation.PopAsync();
    }
}