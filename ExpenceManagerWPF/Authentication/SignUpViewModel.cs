using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ExpenceManager;
using ExpenceManagerModels;
using ExpenceManagerWPF.Navigation;
using Prism.Commands;
using Services;

namespace ExpenceManagerWPF.Authentication
{
    public class SignUpViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private UserReg _regUser = new UserReg();
        private Action _gotoSignIn;
        
        public AuthNavigatableTypes Type
        {
            get
            {
                return AuthNavigatableTypes.SignUp;
            }
        }

        public string Login
        {
            get
            {
                return _regUser.Login;
            }
            set
            {
                if (_regUser.Login != value)
                {
                    _regUser.Login = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get
            {
                return _regUser.Password;
            }
            set
            {
                if (_regUser.Password != value)
                {
                    _regUser.Password = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Name
        {
            get { return _regUser.Name; }
            set
            {
                if (_regUser.Name != value)
                {
                    _regUser.Name = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Surname
        {
            get
            {
                return _regUser.Surname;
            }
            set
            {
                if (_regUser.Surname != value)
                {
                    _regUser.Surname = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get
            {
                return _regUser.Email;
            }
            set
            {
                if (_regUser.Email != value)
                {
                    _regUser.Email = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand SignInCommand { get; }

        public SignUpViewModel(Action gotoSignIn)
        {
            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
            _gotoSignIn = gotoSignIn;
            SignInCommand = new DelegateCommand(_gotoSignIn);
        }

        private async void SignUp()
        {

            var authService = new AuthenticationService();
            try
            {
                await authService.RegisterUserAsync(_regUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign In failed: {ex.Message}");
                return;
            }

            MessageBox.Show($"User successfully registered, please Sign In");
            _gotoSignIn.Invoke();
        }

        private bool IsSignUpEnabled()
        {
            return !String.IsNullOrWhiteSpace(Login) &&
                   !String.IsNullOrWhiteSpace(Password) &&
                   !String.IsNullOrWhiteSpace(Name) &&
                   !String.IsNullOrWhiteSpace(Surname) &&
                   !String.IsNullOrWhiteSpace(Email);
        }

        public void ClearSensitiveData()
        {
            _regUser = new UserReg();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
