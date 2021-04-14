using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace ExpenceManagerWPF.Navigation
{
    public abstract class NavigationBase<TObject> : BindableBase where TObject : Enum
    {
        private List<INavigatable<TObject>> _viewModels = new();

        public INavigatable<TObject> CurrentViewModel
        {
            get;
            private set;
        }

        protected NavigationBase()
        {
        }

        protected void Navigate(TObject type)
        {
            if (CurrentViewModel != null && CurrentViewModel.Type.Equals(type))
            {
                CurrentViewModel.Update();
                return;
            }
                
            INavigatable<TObject> viewModel = _viewModels.FirstOrDefault(authNavigatable => authNavigatable.Type.Equals(type));

            if (viewModel == null)
            {
                viewModel = CreateViewModel(type);
                _viewModels.Add(viewModel);
            }
            viewModel.ClearSensitiveData();
            viewModel.Update();
            CurrentViewModel = viewModel;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }

        protected abstract INavigatable<TObject> CreateViewModel(TObject type);
    }
}