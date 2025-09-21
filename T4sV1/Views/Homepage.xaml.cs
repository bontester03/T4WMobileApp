using T4sV1.Services;
using T4sV1.Services.Security;
namespace T4sV1.Views;

public partial class HomePage : ContentPage
{
    private readonly IAuthService _auth;
    private readonly ISessionService _session;
    private readonly IServiceProvider _sp;

    public HomePage(IAuthService auth, ISessionService session, IServiceProvider sp)
    {
        InitializeComponent();
        _auth = auth;
        _session = session;
        _sp = sp;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            // set true if you want to revoke all devices; false = just this device
            var allDevices = false;

            await _auth.LogoutAsync(allDevices);

            // clear any local session data you keep
            _session.UserId = null;
            _session.Email = null;
            _session.SaveToPreferences();

            // navigate back to LoginPage (resolved via DI)
            var login = _sp.GetRequiredService<LoginPage>();
            Application.Current.MainPage = login;
        }
        catch
        {
            await DisplayAlert("Logout", "Could not logout right now. Please try again.", "OK");
        }
    }

    private async void OnSelectActiveChildClicked(object sender, EventArgs e)
    {
        // Jumps to the "Active Child" tab we routed as "active-child"
        await Shell.Current.GoToAsync("//active-child");
    }
}
