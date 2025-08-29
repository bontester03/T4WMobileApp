using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using T4sV1.Model;
using T4sV1.Services;

namespace T4sV1.Views;

public partial class NotificationsPage : ContentPage
{
    private readonly ISessionService _session;
    public ObservableCollection<NotificationDto> Notifications { get; set; } = new();

    public NotificationsPage()
    {
        InitializeComponent();

        _session = App.ServiceProvider.GetRequiredService<ISessionService>();
        _session.LoadFromPreferences(); // Load stored user ID/email

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadNotificationsAsync();
    }

    private async Task LoadNotificationsAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_session.UserId) || !int.TryParse(_session.UserId, out int userId))
            {
                await DisplayAlert("Error", "User not logged in", "OK");
                return;
            }

            // ✅ Use consistent logic for base URL (live or emulator)
            string baseUrl = "https://time4wellbeing.azurewebsites.net/"; // Default live API

#if DEBUG
            if (DeviceInfo.Platform == DevicePlatform.Android)
                baseUrl = "http://10.0.2.2:5206/";
            else
                baseUrl = "http://localhost:5206/";
#endif

            using var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };

            var response = await client.GetAsync($"api/ApiDashboard/overview/{userId}");



            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"🔔 Notification JSON:\n{json}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ API status code: {response.StatusCode}");
                await DisplayAlert("Error", "Could not fetch notifications.", "OK");
                return;
            }

            var dashboard = JsonSerializer.Deserialize<DashboardDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Notifications.Clear();
            foreach (var note in dashboard.Notifications)
                Notifications.Add(note);
        
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❗ EXCEPTION in LoadNotificationsAsync: {ex}");
            await DisplayAlert("Network Error", "Failed to connect to the server.", "OK");
        }
    }
}
