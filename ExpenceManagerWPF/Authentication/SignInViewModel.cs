using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ExpenceManagerModels;
using ExpenceManagerModels.Users;
using ExpenceManagerWPF.Navigation;
using Prism.Commands;
using Services;

namespace ExpenceManagerWPF.Authentication
{
    public class SignInViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private UserAuth _authUser = new UserAuth();
        private Action _gotoSignUp;
        private Action _gotoWallets;
        private bool _isEnabled = true;


        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public AuthNavigatableTypes Type
        {
            get
            {
                return AuthNavigatableTypes.SignIn;
            }
        }

        public string LoginErr { get; set; }
        public string Login
        {
            get
            {
                return _authUser.Login;
            }
            set
            {
                if (_authUser.Login != value)
                {
                    _authUser.Login = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string PasswordErr { get; set; }
        public string Password
        {
            get
            {
                return _authUser.Password;
            }
            set
            {
                if (_authUser.Password != value)
                {
                    _authUser.Password = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand SignUpCommand { get; }

        public SignInViewModel(Action gotoSignUp, Action gotoWallets)
        {
            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
            _gotoSignUp = gotoSignUp;
            SignUpCommand = new DelegateCommand(_gotoSignUp);
            _gotoWallets = gotoWallets;
        }

        private async void SignIn()
        {
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
                MessageBox.Show("Login or password is empty.");
            else
            {
                var authService = new AuthenticationService();
                var walletService = new WalletService();
                User user = null;
                try
                {
                    IsEnabled = false;
                    await authService.AuthenticateAsync(_authUser);
                    AuthenticationService.CurrentUser =
                        await walletService.LoadUserWallets(AuthenticationService.CurrentUser);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sign In failed: {ex.Message}");
                    return;
                }
                finally
                {
                    IsEnabled = true;
                }
                MessageBox.Show($"Sign In was successful for user {AuthenticationService.CurrentUser.Name} {AuthenticationService.CurrentUser.Surname}");
                _gotoWallets.Invoke();
            }
        }

        private bool IsSignInEnabled()
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(Login))
            {
                LoginErr = "Login can't be empty";
                OnPropertyChanged(nameof(LoginErr));
                valid = false;
            }
            else
            {
                LoginErr = "";
                OnPropertyChanged(nameof(LoginErr));
            }

            if (String.IsNullOrWhiteSpace(Password))
            {
                PasswordErr = "Password can't be empty";
                OnPropertyChanged(nameof(PasswordErr));
                valid = false;
            }
            else
            {
                PasswordErr = "";
                OnPropertyChanged(nameof(PasswordErr));
            }

            return valid;
        }

        public void ClearSensitiveData()
        {
            Password = "";
        }

        public void Update()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
