using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        System.Diagnostics.Debug.WriteLine("ProfilePage constructor called");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine("ProfilePage OnAppearing called");

        // ? ALWAYS reload profile data when page appears
        await _viewModel.LoadProfileAsync();
    }
}