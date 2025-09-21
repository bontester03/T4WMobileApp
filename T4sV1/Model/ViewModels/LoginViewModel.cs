using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using T4sV1.Services;
using T4sV1.Services.Security;

namespace T4sV1.Model.ViewModels
{
    public sealed class LoginViewModel : BindableObject
    {
        private readonly IAuthService _auth;

        public LoginViewModel(IAuthService auth) => _auth = auth;

        private string _email = "";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _password = "";
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        private string _error = "";
        public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }

        public ICommand LoginCommand => new Command(async () => await LoginAsync());

        private async Task LoginAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Error = "";
                var _ = await _auth.LoginAsync(Email.Trim(), Password);
                // navigate to your dashboard page here
                // await Shell.Current.GoToAsync("//DashboardPage");
            }
            catch
            {
                Error = "Invalid email or password.";
            }
            finally { IsBusy = false; }
        }
    }
}
