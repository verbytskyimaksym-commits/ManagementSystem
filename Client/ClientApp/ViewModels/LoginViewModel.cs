using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty] private string email = "kovalenko@clinic.ua";
    [ObservableProperty] private string password = "hash_doctor_1";
    [ObservableProperty] private string errorMessage = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        try
        {
            bool success = await _apiService.LoginAsync(Email, Password);
            if (!success)
            {
                ErrorMessage = "Невірний email або пароль";
                return;
            }

            await Application.Current!.MainPage!.Navigation.PushAsync(new Views.SchedulesPage());
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка з'єднання: {ex.Message}";
        }
    }
}