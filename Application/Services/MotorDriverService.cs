using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class MotorDriverService
    {
        private readonly IMotorDriversRepository _repository;

        public MotorDriverService(IMotorDriversRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MotorDriverDto>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();

            return records.Select(MapToDto).ToList();
        }

        public async Task<MotorDriverDto?> GetBySerialNumberAsync(string serialNumber)
        {
            var record = await _repository.GetBySerialNumberAsync(serialNumber);

            if (record == null)
                return null;

            return MapToDto(record);
        }

        public async Task<MotorDriverDto> AddAsync(CreateMotorDriverDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SerialNumber))
                throw new ArgumentException("Seri numarası boş bırakılamaz.");

            var existingRecord =
                await _repository.GetBySerialNumberAsync(dto.SerialNumber);

            if (existingRecord != null)
            {
                throw new InvalidOperationException(
                    $"'{dto.SerialNumber}' seri numaralı kayıt zaten mevcut.");
            }

            bool isPassed = dto.Voltage >= 23.5 && dto.Voltage <= 24.5;

            var record = new MotorDrivers
            {
                SerialNumber = dto.SerialNumber,
                Voltage = dto.Voltage,
                TestDate = DateTime.UtcNow,
                IsPassed = isPassed
            };

            await _repository.AddAsync(record);

            return MapToDto(record);
        }

        public async Task<MotorDriverDto> UpdateAsync(int id,CreateMotorDriverDto dto)
        {
            var records = await _repository.GetAllAsync();
            var record = records.FirstOrDefault(x => x.Id == id);

            if (record == null)
                throw new InvalidOperationException(
                    $"Id={id} olan kayıt bulunamadı.");

            var sameSerialNumber =
                await _repository.GetBySerialNumberAsync(dto.SerialNumber);

            if (sameSerialNumber != null && sameSerialNumber.Id != id)
            {
                throw new InvalidOperationException(
                    $"'{dto.SerialNumber}' seri numarası başka bir kayıtta zaten kullanılıyor.");
            }

            record.SerialNumber = dto.SerialNumber;
            record.Voltage = dto.Voltage;
            record.IsPassed = dto.Voltage >= 23.5 && dto.Voltage <= 24.5;

            await _repository.UpdateAsync(record);

            return MapToDto(record);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private static MotorDriverDto MapToDto(MotorDrivers record)
        {
            return new MotorDriverDto
            {
                Id = record.Id,
                SerialNumber = record.SerialNumber,
                Voltage = record.Voltage,
                TestDate = record.TestDate,
                IsPassed = record.IsPassed
            };
        }
    }
}