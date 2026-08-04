using MaMoScope_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MaMoScope_.Services
{
    public class MotorDriversStore
    {
        public ObservableCollection<MotorDrivers> Records { get; } = new();
    }
}
