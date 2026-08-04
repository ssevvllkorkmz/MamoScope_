using MaMoScope_.Core;
using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using MaMoScope_.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory (IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<TViewModel>();
        }
        public RecordDetailsViewModel CreateDetailViewModel(MotorDrivers records)
        {
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            var service = _serviceProvider.GetRequiredService<IMotorDriversService>();
            var dialogService = _serviceProvider.GetRequiredService<IDialogService>();

            return new RecordDetailsViewModel(records, navigationService, dialogService, service);
        }
    }
}
