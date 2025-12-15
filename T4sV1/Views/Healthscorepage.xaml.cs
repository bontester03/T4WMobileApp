using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class HealthScorePage : ContentPage
{
    private readonly HealthScoreViewModel _viewModel;

    public HealthScorePage(HealthScoreViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHealthScoresAsync();
    }
}