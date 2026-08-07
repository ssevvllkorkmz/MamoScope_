using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaMoScope_.Core.Interface;
using MaMoScope_.Factories;
using MaMoScope_.Models;
using MaMoScope_.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace MaMoScope_.ViewModels
{
    public partial class PastRecordsViewModel : ViewModelBase
    {

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private MotorDrivers? recordToDelete;

        [ObservableProperty]
        private MotorDrivers selectedRecord;

        [ObservableProperty]
        private bool isDeleteConfirmOpen;

        [ObservableProperty]
        private bool _isAppLoading = false;



        private readonly MotorDriversStore _store;
        private readonly INavigationService _navigationService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IMotorDriversService _service;
        private readonly IDialogService _dialogService;

        private readonly Action<bool> _onLoadingChangedHandler;

        public PastRecordsViewModel(MotorDriversStore store,INavigationService navigationService,IViewModelFactory viewModelFactory,IMotorDriversService service,IDialogService dialogService) 
        {
            _store = store;
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;
            _service = service;
            _dialogService = dialogService;
            _ = LoadAsync();

            _dialogService.OnLoadingChanged += isLoading =>
            {
                IsAppLoading = isLoading;
            };

            _dialogService.OnLoadingChanged += _onLoadingChangedHandler;
        }

        public override void Dispose()
        {
            _dialogService.OnLoadingChanged -= _onLoadingChangedHandler;
        }

        public ObservableCollection<MotorDrivers> Records => _store.Records;

        [RelayCommand]
        private async Task LoadAsync()
        {
            IsLoading = true;
        }

        [RelayCommand]
        private void OpenRecordPage()
        {
            _navigationService.NavigateTo<TestRecordsViewModel>();
        }

        [RelayCommand]

        private void SelectRecord(MotorDrivers record)
        {
            if (record == null) return;

            var detailVm = _viewModelFactory.CreateDetailViewModel(record);
            _navigationService.NavigateTo(detailVm);
        }

        [RelayCommand]
        private void RequestDelete(MotorDrivers record)
        {
            RecordToDelete = record;
            IsDeleteConfirmOpen = true;
        }

        [RelayCommand]
        private async Task ConfirmDeleteAsync()
        {
            if (RecordToDelete == null) return;

            try
            {
                await _service.DeleteAsync(RecordToDelete.Id);
            }
            catch (Exception ex)
            {
                _dialogService.ShowResultDialog($"Silme sırasında hata oluştu:{ex.Message}", false);
            }

            IsDeleteConfirmOpen = false;
            RecordToDelete = null;
        }

        [RelayCommand]
        private void CancelDelete()
        {
            IsDeleteConfirmOpen = false;
            RecordToDelete = null;
        }

    }
}
