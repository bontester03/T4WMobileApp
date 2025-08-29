using T4sV1.Model;
using T4sV1.Services;

namespace T4sV1.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ISessionService _session;
    private readonly ProfileService _profileService = new();

    public PersonalDetailsDto PersonalDetails { get; set; } = new();

    public ProfilePage()
    {
        InitializeComponent();
        _session = App.ServiceProvider.GetRequiredService<ISessionService>();
        _session.LoadFromPreferences();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
         base.OnAppearing();

        if (!int.TryParse(_session.UserId, out int userId)) return;

        var details = await _profileService.GetPersonalDetailsAsync(userId);
        if (details != null)
        PersonalDetails = details;

        BindingContext = null;
        BindingContext = this;
    }
}
