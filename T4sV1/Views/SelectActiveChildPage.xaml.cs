using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class SelectActiveChildPage : ContentPage
{
    public SelectActiveChildPage(SelectActiveChildViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SelectActiveChildViewModel vm)
        {
            _ = vm.LoadAsync();
        }
    }

    private async void OnAddChildClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Add child functionality coming soon", "OK");
    }
}