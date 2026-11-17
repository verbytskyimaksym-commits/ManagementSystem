namespace ClientApp.Models;

public class ScheduleItem
{
    public int ID { get; set; }
    public int DoctorID { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}