// Model/ViewModels/ChildrenListViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model.Children;
using T4sV1.Services;
using T4sV1.Services.Security;

namespace T4sV1.Model.ViewModels;

public sealed class ChildrenListViewModel : INotifyPropertyChanged
{
    private readonly IChildrenService _children;

    public ChildrenListViewModel(IChildrenService children)
    {
        _children = children;
        RefreshCommand = new Command(async () => await LoadAsync());
        DeleteCommand = new Command<ChildDto>(async c => await DeleteAsync(c));
    }

    public ObservableCollection<ChildDto> Items { get; } = new();

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

    public ICommand RefreshCommand { get; }
    public ICommand DeleteCommand { get; }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Items.Clear();
            var list = await _children.ListAsync();
            foreach (var c in list) Items.Add(c);
        }
        finally { IsBusy = false; }
    }

    private async Task DeleteAsync(ChildDto? c)
    {
        if (c is null) return;
        if (await Application.Current.MainPage.DisplayAlert("Delete", $"Delete {c.ChildName}?", "Yes", "No"))
        {
            await _children.DeleteAsync(c.Id);
            Items.Remove(c);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
