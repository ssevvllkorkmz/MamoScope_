using MaMoScope_.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Core.Interface
{
    public interface IMotorDriversService
    {
        (string SerialNumber, double Voltage) SimulateTestData();
        Task<(bool IsSuccessed, string TestResult)> TestandRecordVoltage(string SerialNumber, double Voltage);
        Task<List<MotorDrivers>> GetPastRecordAsync();
        Task UpdateAsync(MotorDrivers record);
        Task DeleteAsync(int id);

    }
}
