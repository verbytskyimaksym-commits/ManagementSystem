using ClientApp.Models;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class AppointmentPage : ContentPage
{
    public AppointmentPage(ScheduleItem schedule)
    {
        InitializeComponent();
        BindingContext = new AppointmentViewModel(schedule);
    }
}