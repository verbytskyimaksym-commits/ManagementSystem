using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClientApp.Models;

namespace ClientApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string TokenKey = "jwt_token";

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5254/")
        };
    }

    private void AttachToken()
    {
        var token = Preferences.Default.Get(TokenKey, string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode)
            return false;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (json.TryGetProperty("token", out var tokenProp))
        {
            var token = tokenProp.GetString() ?? string.Empty;
            Preferences.Default.Set(TokenKey, token);
            AttachToken();
            return true;
        }

        return false;
    }

    public async Task<List<ScheduleItem>> GetSchedulesAsync()
    {
        AttachToken();
        var items = await _httpClient.GetFromJsonAsync<List<ScheduleItem>>("api/schedules");
        return items ?? new List<ScheduleItem>();
    }

    public async Task<bool> CreateAppointmentAsync(int doctorId, int scheduleId, string patientName, string appointmentDate)
    {
        AttachToken();
        var response = await _httpClient.PostAsJsonAsync("api/appointments", new
        {
            doctorID = doctorId,
            scheduleID = scheduleId,
            patientName,
            appointmentDate,
            status = "Confirmed"
        });
        return response.IsSuccessStatusCode;
    }
}