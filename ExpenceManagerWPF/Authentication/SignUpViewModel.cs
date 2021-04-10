using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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


        public string LoginErr { get; set; }
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

        public string PasswordErr { get; set; }
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

        public string NameErr { get; set; }

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

        public string SurnameErr { get; set; }

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

        public string EmailErr { get; set; }
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
            SignUpCommand = new DelegateCommand(SignUp, Validation);
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

        private bool Validation()
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(Login))
            {
                LoginErr = "Login can't be empty";
                OnPropertyChanged(nameof(LoginErr));
                valid = false;
            } else
            if (Login.Length<3)
            {
                LoginErr = "Login can't be less than 3 symbols";
                OnPropertyChanged(nameof(LoginErr));
                valid =  false;
            } else
            if (Login.Length > 10)
            {
                LoginErr = "Login can't be more than 10 symbols";
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
            } else
            if (Password.Length < 6)
            {
                PasswordErr = "Password can't be less than 6 symbols";
                OnPropertyChanged(nameof(PasswordErr));
                valid = false;
            } else
            if (Password.Length > 20)
            {
                PasswordErr = "Password can't be more than 20 symbols";
                OnPropertyChanged(nameof(PasswordErr));
                valid = false;
            }
            else
            {
                PasswordErr = "";
                OnPropertyChanged(nameof(PasswordErr));
            }

            if (String.IsNullOrWhiteSpace(Name))
            {
                NameErr = "Name can't be empty";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            } else
            if (Name.Length > 20)
            {
                NameErr = "Name can't be more than 20 symbols";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            } else
            if (Regex.IsMatch(Name, "[^a-zA-Z]+"))
            {
                NameErr = "Name can contains only latin letters";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            }
            else
            {
                NameErr = "";
                OnPropertyChanged(nameof(NameErr));
            }
            

            if (String.IsNullOrWhiteSpace(Surname))
            {
                SurnameErr = "Surname can't be empty";
                OnPropertyChanged(nameof(SurnameErr));
                valid = false;
            } else
            if (Surname.Length > 20)
            {
                SurnameErr = "Surname can't be more than 20 symbols";
                OnPropertyChanged(nameof(SurnameErr));
                valid = false;
            }else
            if (Regex.IsMatch(Surname, "[^a-zA-Z]+"))
            {
                SurnameErr = "Surname can contains only latin letters";
                OnPropertyChanged(nameof(SurnameErr));
                valid = false;
            }
            else
            {
                SurnameErr = "";
                OnPropertyChanged(nameof(SurnameErr));
            }


            if (String.IsNullOrWhiteSpace(Email))
            {
                EmailErr = "Email can't be empty";
                OnPropertyChanged(nameof(EmailErr));
                valid = false;
            } else
            if (Email.Length > 30)
            {
                EmailErr = "Email can't be more than 30 symbols";
                OnPropertyChanged(nameof(EmailErr));
                valid = false;
            }else
            if (!Regex.IsMatch(Email, ".+@.+"))
            {
                EmailErr = "Email can contains only latin letters and need to have @ symbol inside";
                OnPropertyChanged(nameof(EmailErr));
                valid = false;
            }
            else
            {
                EmailErr = "";
                OnPropertyChanged(nameof(EmailErr));
            }
            return valid;
        }

        public void ClearSensitiveData()
        {
            _regUser = new UserReg();
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
