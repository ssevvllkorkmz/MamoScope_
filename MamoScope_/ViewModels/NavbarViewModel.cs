using CommunityToolkit.Mvvm.Input;
using MaMoScope_.Core.Interface;
using MaMoScope_.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaMoScope_.ViewModels
{
    public partial class NavbarViewModel : ViewModelBase
    {
        private readonly INavigationService _service;

        public NavbarViewModel (INavigationService service)
        {
            _service = service;
        }

        [RelayCommand]
        private void OpenTest()
        {
            _service.NavigateTo<TestRecordsViewModel>();
        }

        [RelayCommand]
        private void OpenPastRecords()
        {
            _service.NavigateTo<PastRecordsViewModel>();
        }

    }
}
