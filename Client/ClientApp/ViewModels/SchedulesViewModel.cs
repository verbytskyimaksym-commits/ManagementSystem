using System.Collections.ObjectModel;
using ClientApp.Models;
using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class SchedulesViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();
    public ObservableCollection<ScheduleItem> Schedules { get; } = new();

    [RelayCommand]
    public async Task LoadSchedulesAsync()
    {
        try
        {
            var items = await _apiService.GetSchedulesAsync();
            Schedules.Clear();
            foreach (var item in items)
                Schedules.Add(item);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenAppointmentAsync(ScheduleItem? schedule)
    {
        if (schedule == null) return;
        await Application.Current!.MainPage!.Navigation.PushAsync(new Views.AppointmentPage(schedule));
    }
}