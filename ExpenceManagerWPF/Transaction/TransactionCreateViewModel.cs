using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExpenceManagerModels;
using ExpenceManagerModels.Wallet;
using ExpenceManagerWPF.Navigation;
using Prism.Commands;
using Services;

namespace ExpenceManagerWPF.Transaction
{
    class TransactionCreateViewModel : INotifyPropertyChanged, INavigatable<WalletsNavigatableTypes>
    {
        private ExpenceManagerModels.Transaction _transaction = new(WalletService.CurrentWallet.Currency);
        private Action _gotoWallets;


        public List<string> PossibleCurrency => Wallet.PossibleCurrency.Keys.ToList();

        public string CurrencyErr { get; set; }
        public string Currency
        {
            get
            {
                return _transaction.Currency;
            }
            set
            {
                _transaction.Currency = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }


        public string CurrentCategoryErr { get; set; }
        public Category CurrentCategory
        {
            get
            {
                return _transaction.Category;
            }
            set
            {
                _transaction.Category = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Category> Categories
        {
            get
            {
                return AuthenticationService.CurrentUser.Categories;
            }
        }

        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand AddTransactionCommand { get; }
        public TransactionCreateViewModel(Action gotoWallets)
        {
            _gotoWallets = gotoWallets;
            GoBackCommand = new DelegateCommand(_gotoWallets);
            AddTransactionCommand = new DelegateCommand(CreateTransaction, IsValid);
        }


        public string AmountErr { get; set; }
        public decimal Amount
        {
            get
            {
                return _transaction.Amount;
            }
            set
            {
                _transaction.Amount = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }
        
        public string DescriptionErr { get; set; }
        public string Description
        {
            get
            {
                return _transaction.Description;
            }
            set
            {
                _transaction.Description = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        
        public string DateTimeErr { get; set; }
        public DateTime DateTime
        {
            get
            {
                return _transaction.DateTime;
            }
            set
            {
                _transaction.DateTime = value;
                OnPropertyChanged();
            }
        }
        
        
        private bool IsValid()
        {
            bool valid = true;

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

            if (CurrentCategory == null)
            {
                CurrentCategoryErr = "Category is necessary";
                OnPropertyChanged(nameof(CurrentCategoryErr));
                valid = false;
            }
            else
            {
                CurrentCategoryErr = "";
                OnPropertyChanged(nameof(CurrentCategoryErr));
            }
            if (String.IsNullOrWhiteSpace(Currency))
            {
                CurrencyErr = "Choose currency";
                OnPropertyChanged(nameof(CurrencyErr));
                valid = false;
            }
            else
            {
                CurrencyErr = "";
                OnPropertyChanged(nameof(CurrencyErr));
            }



            return valid;
        }

        private async void CreateTransaction()
        {

            var service = new TransactionService();
            try
            {
                WalletService.CurrentWallet.AddTransaction(AuthenticationService.CurrentUser, _transaction);
                await service.SaveUpdateTransaction(WalletService.CurrentWallet, _transaction);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add transaction failed: {ex.Message}");
                return;
            }
            
            MessageBox.Show($"Transaction added");
            _gotoWallets.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WalletsNavigatableTypes Type
        {
            get
            {
                return WalletsNavigatableTypes.TransactionCreation;
            }
        }
        public void ClearSensitiveData()
        {
            _transaction = new ExpenceManagerModels.Transaction(WalletService.CurrentWallet.Currency);
        }

        public void Update()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
