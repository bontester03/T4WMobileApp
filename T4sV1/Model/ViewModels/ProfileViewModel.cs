using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model.Children;
using T4sV1.Model.Profile;
using T4sV1.Services;
using T4sV1.Services.Http;
using T4sV1.Services.Security;
using T4sV1.Model.Enums;

namespace T4sV1.Model.ViewModels;

public sealed class ProfileViewModel : INotifyPropertyChanged
{
    private readonly IProfileService _profileService;
    private readonly IActiveChildStore _activeChildStore;
    private readonly ISessionService _sessionService;

    public ProfileViewModel(
        IProfileService profileService,
        IActiveChildStore activeChildStore,
        ISessionService sessionService)
    {
        _profileService = profileService;
        _activeChildStore = activeChildStore;
        _sessionService = sessionService;

        // 🧪 Debug logging - matching HomePageViewModel pattern
        System.Diagnostics.Debug.WriteLine("🏗️ ProfileViewModel Constructor Called");
        System.Diagnostics.Debug.WriteLine($"   - ProfileService: {_profileService != null}");
        System.Diagnostics.Debug.WriteLine($"   - ActiveChildStore: {_activeChildStore != null}");
        System.Diagnostics.Debug.WriteLine($"   - SessionService: {_sessionService != null}");

        ChangeTabCommand = new Command<string>(OnChangeTab);
        BackCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        UpdateAvatarCommand = new Command(async () => await UpdateAvatarAsync());
        SaveCommand = new Command(async () => await SaveAsync());
        RefreshCommand = new Command(async () => await LoadProfileAsync());

        Child = new ChildDto();
        ParentInfo = new Profile.PersonalDetailsDto();
    }

    #region Properties

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    private string _activeTab = "profile";
    public string ActiveTab
    {
        get => _activeTab;
        set
        {
            _activeTab = value;
            OnPropertyChanged();
        }
    }

    private ChildDto _child;
    public ChildDto Child
    {
        get => _child;
        set
        {
            _child = value;
            OnPropertyChanged();
        }
    }

    private Profile.PersonalDetailsDto _parentInfo;
    public Profile.PersonalDetailsDto ParentInfo
    {
        get => _parentInfo;
        set
        {
            _parentInfo = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> GenderOptions { get; } = new()
    {
        "Male",
        "Female",
        "Other"
    };

    private string _selectedGender = "Male";
    public string SelectedGender
    {
        get => _selectedGender;
        set
        {
            _selectedGender = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand ChangeTabCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand UpdateAvatarCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand RefreshCommand { get; }

    #endregion

    #region Methods

    public async Task LoadProfileAsync()
    {
        //if (IsBusy) return;

        try
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("=== ProfileViewModel.LoadProfileAsync started ===");

            // 🎯 Get active child ID - EXACTLY like HomePageViewModel does it
            var activeChildId = await _activeChildStore.GetAsync();

            System.Diagnostics.Debug.WriteLine($"🔍 Active Child ID from Store: {activeChildId?.ToString() ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"🔍 Session Active Child ID: {_sessionService.ActiveChildId?.ToString() ?? "NULL"}");

            // If store is empty, try session (backup)
            if (!activeChildId.HasValue)
            {
                activeChildId = _sessionService.ActiveChildId;
                System.Diagnostics.Debug.WriteLine($"🔄 Using Session ActiveChildId: {activeChildId}");
            }

            System.Diagnostics.Debug.WriteLine($"🎯 Calling API with ActiveChildId: {activeChildId?.ToString() ?? "NONE"}");

            // Call API with active child ID
            var profile = await _profileService.GetProfileAsync(activeChildId).ConfigureAwait(false);

            if (profile == null)
            {
                System.Diagnostics.Debug.WriteLine("❌ Profile API returned NULL");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "Failed to load profile", "OK");
                });
                return;
            }

            // Update UI on main thread
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (profile.Child != null)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Loaded Profile for Child:");
                    System.Diagnostics.Debug.WriteLine($"   - ID: {profile.Child.Id}");
                    System.Diagnostics.Debug.WriteLine($"   - Name: {profile.Child.ChildName}");
                    System.Diagnostics.Debug.WriteLine($"   - Gender: {profile.Child.Gender}");
                    System.Diagnostics.Debug.WriteLine($"   - Age: {profile.Child.Age}");

                    Child = profile.Child;
                    SelectedGender = profile.Child.Gender.ToString();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Profile.Child is NULL");
                }

                if (profile.ParentInfo != null)
                {
                    ParentInfo = profile.ParentInfo;
                    System.Diagnostics.Debug.WriteLine($"✅ Parent Info: School={ParentInfo.School}");
                }

                System.Diagnostics.Debug.WriteLine("=== ProfileViewModel.LoadProfileAsync completed ===");
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ ERROR in LoadProfileAsync:");
            System.Diagnostics.Debug.WriteLine($"   Type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"   Message: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"   StackTrace: {ex.StackTrace}");

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
            });
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }
    }

    private void OnChangeTab(string tab)
    {
        ActiveTab = tab;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("Tab Changed", $"Switched to {tab} tab", "OK");
        });
    }

    private async Task UpdateAvatarAsync()
    {
        try
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Avatar",
                "Enter avatar URL (for testing):",
                placeholder: "https://example.com/avatar.jpg");

            if (string.IsNullOrWhiteSpace(result))
                return;

            IsBusy = true;

            var request = new UpdateAvatarDto { AvatarUrl = result };
            var success = await _profileService.UpdateAvatarAsync(request).ConfigureAwait(false);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (success)
                {
                    Child.AvatarUrl = result;
                    OnPropertyChanged(nameof(Child));
                    await Application.Current.MainPage.DisplayAlert("Success", "Avatar updated!", "OK");

                    // Reload to get fresh data
                    await LoadProfileAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update avatar", "OK");
                }
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
            });
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("=== SaveAsync started ===");
            System.Diagnostics.Debug.WriteLine($"📋 Child object state:");
            System.Diagnostics.Debug.WriteLine($"   - Child is null: {Child == null}");
            System.Diagnostics.Debug.WriteLine($"   - Child.Id: {Child?.Id ?? 0}");
            System.Diagnostics.Debug.WriteLine($"   - Child.ChildName: {Child?.ChildName ?? "NULL"}");

            if (Child == null || Child.Id == 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ ERROR: Child.Id is 0 or Child is null!");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Cannot save: Child ID is {Child?.Id ?? 0}. Please reload the profile.",
                        "OK");
                });
                return;
            }

            Enum.TryParse<Gender>(SelectedGender, out var gender);

            var request = new UpdateProfileRequest
            {
                ChildId = Child.Id,
                Child = new UpdateChildDto
                {
                    ChildName = Child.ChildName,
                    Gender = gender,
                    DateOfBirth = Child.DateOfBirth
                },
                ParentInfo = new Profile.PersonalDetailsDto
                {
                    School = ParentInfo.School,
                    Class = ParentInfo.Class,
                    ParentGuardianName = ParentInfo.ParentGuardianName,
                    RelationshipToChild = ParentInfo.RelationshipToChild,
                    TeleNumber = ParentInfo.TeleNumber,
                    Email = ParentInfo.Email,
                    Postcode = ParentInfo.Postcode
                }
            };

            System.Diagnostics.Debug.WriteLine($"🌐 Request object created:");
            System.Diagnostics.Debug.WriteLine($"   - request.ChildId: {request.ChildId}");
            System.Diagnostics.Debug.WriteLine($"   - request.Child.ChildName: {request.Child.ChildName}");

            var success = await _profileService.UpdateProfileAsync(request).ConfigureAwait(false);

            // ... rest of the code
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ ERROR in SaveAsync: {ex.Message}");
            // ...
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }
    }

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}