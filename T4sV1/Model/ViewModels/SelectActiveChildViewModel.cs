using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model.Children;
using T4sV1.Services;
using T4sV1.Services.Security;

namespace T4sV1.Model.ViewModels;

public sealed class SelectActiveChildViewModel : INotifyPropertyChanged
{
    private readonly IChildrenService _children;
    private readonly IActiveChildStore _active;

    public SelectActiveChildViewModel(IChildrenService children, IActiveChildStore active)
    {
        _children = children;
        _active = active;
        SelectCommand = new Command<ChildDto>(async c => await SelectAsync(c));
        RefreshCommand = new Command(async () => await LoadAsync());
    }

    public ObservableCollection<ChildDto> Items { get; } = new();

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

    private int? _currentId;
    public int? CurrentId { get => _currentId; set { _currentId = value; OnPropertyChanged(); } }

    public ICommand SelectCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            CurrentId = await _active.GetAsync();
            Items.Clear();
            var list = await _children.ListAsync();
            foreach (var c in list) Items.Add(c);
        }
        finally { IsBusy = false; }
    }

    private async Task SelectAsync(ChildDto? child)
    {
        if (child is null) return;
        await _active.SetAsync(child.Id);
        CurrentId = child.Id;

        // Optional toast
        await Application.Current.MainPage.DisplayAlert("Active child", $"{child.ChildName} selected.", "OK");

        // Go back to Dashboard tab if you want:
        await Shell.Current.GoToAsync("//dashboard");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}
