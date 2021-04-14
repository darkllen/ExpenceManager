using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExpenceManagerModels;
using ExpenceManagerWPF.Navigation;
using ExpenceManagerWPF.Wallets;
using Prism.Commands;
using Services;

namespace ExpenceManagerWPF.Categories
{
    class CategoryViewModel : INotifyPropertyChanged, INavigatable<WalletsNavigatableTypes>
    {
        private Category _category = new Category();

        private Category _currentCategory;

        private Action _goBack;

        public DelegateCommand GoBack { get; }

        public ObservableCollection<Category> Categories { get; set; }

        public Category CurrentCategory
        {
            get
            {
                return _currentCategory;
            }
            set
            {
                _currentCategory = value;
                OnPropertyChanged();
                RemoveCategoryCommand.RaiseCanExecuteChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                return _category.Name;
            }
        }

        public string NameErr { get; set; }
        public string Name
        {
            get
            {
                return _category.Name;
            }
            set
            {
                _category.Name = value;
                OnPropertyChanged();
                AddCategoryCommand.RaiseCanExecuteChanged();
            }
        }

        public string DescriptionErr { get; set; }
        public string Description
        {
            get
            {
                return _category.Description;
            }
            set
            {
                _category.Description = value;
                OnPropertyChanged();
                AddCategoryCommand.RaiseCanExecuteChanged();
            }
        }

        private async void CreateCategory()
        {
            var service = new UserService();
            try
            {
                AuthenticationService.CurrentUser.Categories.Add(_category);
                await service.RecordCategories(AuthenticationService.CurrentUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add category failed: {ex.Message}");
                return;
            }
            MessageBox.Show($"Category added");
            _goBack.Invoke();
        }

        private async void RemoveCategory()
        {
            var service = new UserService();
            var tservice = new TransactionService();
            try
            {
                AuthenticationService.CurrentUser.Categories.Remove(_currentCategory);
                await service.RecordCategories(AuthenticationService.CurrentUser);

                foreach (var wallet in AuthenticationService.CurrentUser.Wallets)
                {
                    foreach (var transaction in wallet.GetAllTransactions(AuthenticationService.CurrentUser))
                    {
                        if (Equals(transaction.Category, _currentCategory))
                        {
                            transaction.Category = Category.DefaultCategory;
                            await tservice.SaveUpdateTransaction(wallet, transaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Remove category failed: {ex.Message}");
                return;
            }
            MessageBox.Show($"Category deleted");
            _goBack.Invoke();
        }


        private bool IsValid()
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(Name))
            {
                NameErr = "Name can't be empty";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            }
            else if (Name.Length > 20)
            {
                NameErr = "Name can't be more than 20 symbols";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            }
            else
            {
                NameErr = "";
                OnPropertyChanged(nameof(NameErr));
            }

            if (String.IsNullOrWhiteSpace(Description))
            {
                DescriptionErr = "Description can't be empty";
                OnPropertyChanged(nameof(DescriptionErr));
                valid = false;
            }
            else
            {
                DescriptionErr = "";
                OnPropertyChanged(nameof(DescriptionErr));
            }

            return valid;
        }

        public WalletsNavigatableTypes Type
        {
            get
            {
                return WalletsNavigatableTypes.CategoryCreation;
            }
        }

        public DelegateCommand AddCategoryCommand { get; }

        public DelegateCommand RemoveCategoryCommand { get; }
        public CategoryViewModel(Action goBack)
        {

            Categories = new ObservableCollection<Category>();

            foreach (var category in AuthenticationService.CurrentUser.Categories)
            {
                Categories.Add(category);
            }

            _goBack = goBack;
            GoBack = new DelegateCommand(_goBack);
            AddCategoryCommand = new DelegateCommand(CreateCategory, IsValid);
            RemoveCategoryCommand = new DelegateCommand(RemoveCategory, IsValidRemove);
        }

        private bool IsValidRemove()
        {
            return _currentCategory != null && !Equals(_currentCategory, Category.DefaultCategory);
        }


        public void ClearSensitiveData()
        {
        }

        public void Update()
        {
            Categories = new ObservableCollection<Category>();

            foreach (var category in AuthenticationService.CurrentUser.Categories)
            {
                Categories.Add(category);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
