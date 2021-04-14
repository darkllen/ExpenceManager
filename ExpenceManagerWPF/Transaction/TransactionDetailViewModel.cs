using System;
using System.Collections.Generic;
using System.Windows;
using ExpenceManagerModels;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace ExpenceManagerWPF.Transaction
{
    public class TransactionDetailViewModel : BindableBase
    {
        private ExpenceManagerModels.Transaction _transaction;

        private Action _update;


        public ExpenceManagerModels.Transaction Transaction
        {
            get
            {
                return _transaction;
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
                RaisePropertyChanged();
            }
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

                RaisePropertyChanged(nameof(DisplayName));
                UpdateTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand UpdateTransactionCommand { get; }


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
                RaisePropertyChanged(nameof(DisplayName));
                UpdateTransactionCommand.RaiseCanExecuteChanged();
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
                RaisePropertyChanged();
                UpdateTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Category> Categories
        {
            get
            {
                return AuthenticationService.CurrentUser.Categories;
            }
        }



        public string DisplayName
        {
            get
            {
                return $"{_transaction.Amount} {_transaction.Currency}";
            }
        }

        public TransactionDetailViewModel(ExpenceManagerModels.Transaction transaction, Action update)
        {
            _transaction = transaction;
            _update = update;
            UpdateTransactionCommand = new DelegateCommand(UpdateTransaction, IsValid);
        }

        private bool IsValid()
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(Description))
            {
                DescriptionErr = "Description can't be empty";
                RaisePropertyChanged(nameof(DescriptionErr));
                valid = false;
            }
            else
            {
                DescriptionErr = "";
                RaisePropertyChanged(nameof(DescriptionErr));
            }

            if (CurrentCategory == null)
            {
                CurrentCategoryErr = "Category is necessary";
                RaisePropertyChanged(nameof(CurrentCategoryErr));
                valid = false;
            }
            else
            {
                CurrentCategoryErr = "";
                RaisePropertyChanged(nameof(CurrentCategoryErr));
            }

            return valid;
        }


        private async void UpdateTransaction()
        {

            var service = new TransactionService();
            try
            {
                WalletService.CurrentWallet.RemoveTransaction(AuthenticationService.CurrentUser, _transaction);
                await service.SaveUpdateTransaction(WalletService.CurrentWallet, _transaction);
                WalletService.CurrentWallet.AddTransaction(AuthenticationService.CurrentUser, _transaction);
                _update.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update transaction failed: {ex.Message}");
                return;
            }

            MessageBox.Show($"transaction updated");
            
        }
    }
}
