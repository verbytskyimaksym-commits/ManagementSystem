using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SchedulesPage : ContentPage
{
    private readonly SchedulesViewModel _viewModel = new();

    public SchedulesPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSchedulesCommand.ExecuteAsync(null);
    }
}