using System;
using System.Collections.Generic;
using System.Text;
using MaMoScope_.ViewModels;

namespace MaMoScope_.Core.Interface
{
    public interface INavigationService
    {
        object? CurrentView { get; }
        event Action? CurrentViewChanged;

        void NavigateTo<TViewModel>() where TViewModel : notnull;

        void NavigateTo(ViewModelBase viewModel);
    }
}
