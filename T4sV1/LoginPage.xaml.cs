using System.Text;
using System.Text.Json;
using T4sV1.Model;
using T4sV1.Services;

namespace T4sV1;

public partial class LoginPage : ContentPage
{
    private readonly ISessionService _session;

    public LoginPage(ISessionService session)
    {
        InitializeComponent();
        _session = session;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetNavBarIsVisible(this, false);
    }


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Validation", "Please enter both email and password.", "OK");
            return;
        }

        try
        {
            var loginDto = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            // Local testing:
            //var response = await client.PostAsync("http://10.0.2.2:5206/api/ApiAuth/login", content);

            //var response = await client.PostAsync("http://192.168.1.164:5206/api/ApiAuth/login", content);

            // Live server:
           var response = await client.PostAsync("https://time4wellbeing.azurewebsites.net/api/ApiAuth/login", content);
            //var response = await client.PostAsync("api/ApiAuth/login", content);


            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Raw Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {

                await DisplayAlert("Login Failed", $"Server says: {responseBody}", "OK");
                return;
            }

            var user = JsonSerializer.Deserialize<LoginResponseDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (user == null || user.UserId == 0)
            {
                await DisplayAlert("Login Error", "User object is null or invalid.", "OK");
                return;
            }
            Application.Current.MainPage = new AppShell(); // ✅ Now use Shell and full nav

            await DisplayAlert("Success", $"Welcome {user.Email}", "OK");
            await SecureStorage.Default.SetAsync("UserId", user.UserId.ToString());
            _session.UserId = user.UserId.ToString();
            _session.Email = user.Email;
            _session.SaveToPreferences();



            // ✅ Since you're still in LoginPage (not AppShell), switch MainPage here:
            Application.Current.MainPage = new AppShell();

            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
            await DisplayAlert("Connection Error", ex.Message, "OK");
        }
    }
}
