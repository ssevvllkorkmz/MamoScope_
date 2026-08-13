using MaMoScope_API.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MamoScope_API.Data
{
    public interface IMotorDriversRepository
    {
        Task<List<MotorDrivers>> GetAllAsync();
        Task<MotorDrivers?> GetBySerialNumberAsync(string SerialNumber);
        Task AddAsync(MotorDrivers record);
        Task UpdateAsync(MotorDrivers record);
        Task DeleteAsync(int id);
    }
}