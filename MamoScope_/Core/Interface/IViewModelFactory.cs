using MaMoScope_.Models;
using MaMoScope_.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Core.Interface
{
    public interface IViewModelFactory
    {
        RecordDetailsViewModel CreateDetailViewModel(MotorDrivers records);
    }
}
