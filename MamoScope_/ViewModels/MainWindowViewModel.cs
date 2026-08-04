using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaMoScope_.Core.Interface;
using MaMoScope_.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaMoScope_.ViewModels
{
    
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public NavbarViewModel NavbarViewModel { get; }

        [ObservableProperty]
        private object _currentView;



        public MainWindowViewModel(INavigationService navigationService, IDialogService dialogService,NavbarViewModel navbarViewModel)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            NavbarViewModel = navbarViewModel;

            _navigationService.CurrentViewChanged += OnCurrentViewChanged;

        }

       
        private void OnCurrentViewChanged()
        {
           
            CurrentView = _navigationService.CurrentView;
           
        }
   
    }
}
