using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SchedulesPage : ContentPage
{
    public SchedulesPage()
    {
        InitializeComponent();
        BindingContext = new SchedulesViewModel();
    }
}