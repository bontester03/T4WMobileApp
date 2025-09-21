using Microsoft.Maui.Storage;
using System.Text.Json;
using T4sV1.Model;            // your ISessionService lives here (from your code)
using T4sV1.Services;          // IAuthService
using T4sV1.Services.Security;

namespace T4sV1;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _auth;
    private readonly ISessionService _session;
    private bool _isBusy;

    public LoginPage(IAuthService auth, ISessionService session)
    {
        InitializeComponent();
        _auth = auth;
        _session = session;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetNavBarIsVisible(this, false);
    }

    // Prefer commands with async Task, but keeping your signature for now
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (_isBusy) return;

        var email = emailEntry?.Text?.Trim() ?? string.Empty;
        var password = passwordEntry?.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Validation", "Please enter both email and password.", "OK");
            return;
        }

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        _isBusy = true;
        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                throw new InvalidOperationException("No internet connection.");

            var result = await _auth.LoginAsync(email, password, cts.Token);

            if (result == null)
                throw new InvalidOperationException("Login failed: empty response from server.");

            // Validate the fields your API guarantees
            if (string.IsNullOrWhiteSpace(result.Id) || string.IsNullOrWhiteSpace(result.Email))
                throw new InvalidOperationException("Login failed: invalid credentials or incomplete response.");

            _session.UserId = result.Id;
            _session.Email = result.Email;
            _session.SaveToPreferences();

            try { await SecureStorage.Default.SetAsync("UserId", result.Id); }
            catch (Exception sse) { System.Diagnostics.Debug.WriteLine($"SecureStorage failed: {sse.Message}"); }

            Application.Current.MainPage = new AppShell();
            await DisplayAlert("Success", $"Welcome {result.Email}", "OK");
        }
        catch (TaskCanceledException)
        {
            await DisplayAlert("Timeout", "The server didn’t respond in time. Please try again.", "OK");
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Network error", $"Could not reach the server.\n{ex.Message}", "OK");
        }
        catch (JsonException jex)
        {
            await DisplayAlert("Parse error", $"Unexpected server response.\n{jex.Message}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Login error", ex.Message, "OK");
        }
        finally
        {
            _isBusy = false;
        }
    }


}
