using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using MaMoScope_.Services;
using MaMoScope_.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Navigation;


namespace MaMoScope_.ViewModels
{
    public partial class TestRecordsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string serialNumber;

        [ObservableProperty]
        private string voltageValue;

        [ObservableProperty]
        private string result;

        [ObservableProperty]
        private string date;

        [ObservableProperty]
        private string resultMessage;

        [ObservableProperty]
        private bool resultIsSucces;

        [ObservableProperty]
        private bool isResultDialogOpen = false;

        private readonly IDialogService _dialogService;
        private readonly IMotorDriversService _service;
        private readonly INavigationService _navigationService;

        private readonly Action<string, bool> _onShowDialogHandler;
        private readonly Action _onCloseDialogHandler;

        public TestRecordsViewModel(IDialogService dialogService, IMotorDriversService service, INavigationService navigationservice)
        {
            _dialogService = dialogService;
            _service = service;
            _navigationService = navigationservice;
            date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            _dialogService.OnShowDialog += (message, IsSuccessed) =>
            {
                ResultMessage = message;
                ResultIsSucces = IsSuccessed;
                IsResultDialogOpen = true;
            };

            _dialogService.OnCloseDialog += () =>
            {
                IsResultDialogOpen = false;
            };

            _dialogService.OnShowDialog += _onShowDialogHandler;
            _dialogService.OnCloseDialog += _onCloseDialogHandler;

        }
        public override void Dispose()
        {
            _dialogService.OnShowDialog -= _onShowDialogHandler;
            _dialogService.OnCloseDialog -= _onCloseDialogHandler;
        }

        [RelayCommand]
        private async Task TestandRecordVoltage()
        {
            if (string.IsNullOrEmpty(voltageValue) || string.IsNullOrEmpty(serialNumber))
            {
                _dialogService.ShowResultDialog("Lütfen önce voltaj ve seri numarası değerleri girin!", false);
                return;
            }

            string voltageMessage = voltageValue.Trim().Replace(",", ".");

            if (!double.TryParse(voltageMessage, NumberStyles.Any, CultureInfo.InvariantCulture, out double reelVoltage))
            {
                _dialogService.ShowResultDialog("Geçerli bir voltaj değeri girin! (ÖR:24.5)", false);
                return;
            }

            try
            {
                var (IsSuccessed, result) = await _service.TestandRecordVoltage(serialNumber, reelVoltage);

                ResultMessage = $"Seri No: {serialNumber}\nVoltaj: {reelVoltage}V\nSonuç: {result}";
                IsResultDialogOpen = true;
            }

            catch (Exception ex)
            {
                ResultMessage = $"Hata: {ex.Message}";
                IsResultDialogOpen = true;
            }
        }

        [RelayCommand]
        private void SimulateTestData()
        {
            var (newSerialNumber, newVoltage) = _service.SimulateTestData();
            SerialNumber = newSerialNumber;
            VoltageValue = newVoltage.ToString(CultureInfo.InvariantCulture);
        }


        [RelayCommand]
        private async Task OpenPastRecord()
        {
            await Task.Delay(50);
            _navigationService.NavigateTo<PastRecordsViewModel>();
        }
    

    [RelayCommand]
        private void CloseResultDialog()
        {
            _dialogService.CloseDialog();
        }

    }
}


