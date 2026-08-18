using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
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