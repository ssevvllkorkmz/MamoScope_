using MaMoScope_.Core.Interface;
using MaMoScope_.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaMoScope_.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object? _currentView;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public object? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                CurrentViewChanged?.Invoke();
            }
        }

        public event Action? CurrentViewChanged;

        public void NavigateTo<TViewModel>() where TViewModel : notnull
        {
            if (CurrentView is IDisposable disposable)
                disposable.Dispose();
            CurrentView = _serviceProvider.GetRequiredService<TViewModel>();
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            if (CurrentView is IDisposable disposable)
                disposable.Dispose();
            CurrentView = viewModel;
        }
    }
}
