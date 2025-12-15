using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class MeasurementPage : ContentPage
{
    private readonly MeasurementViewModel _viewModel;

    public MeasurementPage(MeasurementViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMeasurementsAsync();
    }
}