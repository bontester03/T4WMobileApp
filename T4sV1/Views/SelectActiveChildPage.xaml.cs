using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class SelectActiveChildPage : ContentPage
{
    private readonly SelectActiveChildViewModel _vm;

    public SelectActiveChildPage(SelectActiveChildViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
