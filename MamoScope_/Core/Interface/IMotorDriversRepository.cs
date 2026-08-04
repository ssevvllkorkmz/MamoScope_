using MaMoScope_.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Core.Interface
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
