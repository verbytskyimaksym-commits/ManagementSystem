using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string email = "kovalenko@clinic.ua";
    [ObservableProperty] private string password = "hash_doctor_1";
    [ObservableProperty] private string errorMessage = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введіть email та пароль";
            return;
        }

        ErrorMessage = string.Empty;
        await Application.Current!.MainPage!.Navigation.PushAsync(new Views.SchedulesPage());
    }
}