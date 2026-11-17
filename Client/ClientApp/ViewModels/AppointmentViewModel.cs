using ClientApp.Models;
using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class AppointmentViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

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

        bool ok = await _apiService.CreateAppointmentAsync(DoctorId, ScheduleId, PatientName, AppointmentDate);
        if (ok)
        {
            await Application.Current!.MainPage!.DisplayAlert("Успіх", "Запис підтверджено та збережено в БД!", "OK");
            await Application.Current!.MainPage!.Navigation.PopAsync();
        }
        else
        {
            StatusMessage = "Не вдалося зберегти запис.";
        }
    }
}