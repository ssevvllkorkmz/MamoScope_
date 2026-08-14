using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using MaMoScope_.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MaMoScope_.ViewModels
{
    public partial class RecordDetailsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private MotorDrivers editableRecord;

        [ObservableProperty]
        private string voltageValueText;

        [ObservableProperty]
        private bool isResultDialogOpen;

        [ObservableProperty]
        private string resultMessage;

        [ObservableProperty]
        private bool resultIsSucces;


        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IMotorDriversService _service;

        private readonly Action<string, bool> _onShowDialogHandler;
        private readonly Action _onCloseDialogHandler;

        public RecordDetailsViewModel (MotorDrivers selectedRecord,INavigationService navigationService,IDialogService dialogService,IMotorDriversService service)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _service = service;

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

            EditableRecord = new MotorDrivers
            {
                Id = selectedRecord.Id,
                Voltage = selectedRecord.Voltage,
                SerialNumber = selectedRecord.SerialNumber,
                TestDate = DateTime.SpecifyKind(selectedRecord.TestDate, DateTimeKind.Utc).ToLocalTime(),
                IsPassed = selectedRecord.IsPassed
            };

            VoltageValueText = selectedRecord.Voltage.ToString(CultureInfo.InvariantCulture);
        }

        public override void Dispose()
        {
            _dialogService.OnShowDialog -= _onShowDialogHandler;
            _dialogService.OnCloseDialog -= _onCloseDialogHandler;
        }

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.NavigateTo<PastRecordsViewModel>();
        }

        [RelayCommand]
        private async Task SaveChangesAsync()
        {
           
            if (string.IsNullOrWhiteSpace(EditableRecord.SerialNumber))
            {
                _dialogService.ShowResultDialog("Seri numarası boş olamaz!", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(VoltageValueText))
            {
                _dialogService.ShowResultDialog("Voltaj değeri boş olamaz!", false);
                return;
            }

            string newText = VoltageValueText.Trim().Replace(',', '.');
            if (!double.TryParse(newText, NumberStyles.Any, CultureInfo.InvariantCulture, out double reelVoltage))
            {
                _dialogService.ShowResultDialog("Geçerli bir voltaj değeri girin! ÖR:24.5", false);
                return;
            }

            EditableRecord.Voltage = reelVoltage;

            try
            {
                await _service.UpdateAsync(EditableRecord);
                _navigationService.NavigateTo<PastRecordsViewModel>();
            }
            catch (InvalidOperationException ex)
            {
                _dialogService.ShowResultDialog(ex.Message, false);
            }
            catch (Exception ex)
            {
                _dialogService.ShowResultDialog($"Güncelleme sırasında hata oluştu: {ex.Message}", false);

            }
        }

        [RelayCommand]
        private void CloseResultDialog()
        {
            IsResultDialogOpen = false;
        }

    }
}
