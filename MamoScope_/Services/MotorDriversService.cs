using MaMoScope_.Core;
using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace MaMoScope_.Services
{
    public partial class MotorDriversService : IMotorDriversService
    {
        private readonly IMotorDriversRepository _repository;
        private readonly MotorDriversStore _store;

        private static readonly Regex SeriNumarasiFormati = new Regex(@"^OPT-DRV-\d{4}$", RegexOptions.Compiled);

        public MotorDriversService(IMotorDriversRepository repository,MotorDriversStore store)
        {
            _repository = repository;
            _store = store;
        }

        private void  VerifySerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber) || !SeriNumarasiFormati.IsMatch(serialNumber))
            {
                throw new InvalidOperationException("Seri numarası 'OPT-DRV-XXXX' formatında olmalıdır (örnek: OPT-DRV-4521).");
            }
        }

        public (string SerialNumber,double Voltage) SimulateTestData()
        {
            var rnd = new Random();
            int randomNumber = rnd.Next(1000, 9999);
            string serialNumber = $"OPT-DRV-{randomNumber}";

            double newVoltage = 20.0 + (rnd.NextDouble() * 6.0);
            newVoltage = Math.Round(newVoltage, 1);

            return (serialNumber, newVoltage);
        }

        public async Task<(bool IsSuccessed, string TestResult)> TestandRecordVoltage(string SerialNumber,double Voltage)
        {
            VerifySerialNumber(SerialNumber);
            var currentRecord = await _repository.GetBySerialNumberAsync(SerialNumber);
            if (currentRecord != null)
            {
                throw new InvalidOperationException($"'{SerialNumber}' seri numaralı kayıt zaten mevcut. Lütfen yeni bir test simüle edin.");
            }

            bool IsSuccessed = Voltage >= 23.5 && Voltage <= 24.5;
            string TestResult = IsSuccessed ? "Başarılı" : "Başarısız";

            var newRecord = new MotorDrivers
            {
                SerialNumber = SerialNumber,
                Voltage = Voltage,
                IsPassed = IsSuccessed,
                TestDate = DateTime.Now
            };

            await _repository.AddAsync(newRecord);
            _store.Records.Insert(0, newRecord);
            return (IsSuccessed, TestResult);
        } 

        public Task<List<MotorDrivers>> GetPastRecordAsync()
        {
            return _repository.GetAllAsync();
        }

        public async Task LoadAsync()
        {
            var list = await _repository.GetAllAsync();

            _store.Records.Clear();
            foreach (var record in list)
            _store.Records.Add(record);
        }

        public async Task UpdateAsync(MotorDrivers record)
        {

            VerifySerialNumber(record.SerialNumber);

            var sameSerialNumber = await _repository.GetBySerialNumberAsync(record.SerialNumber);
            if (sameSerialNumber != null && sameSerialNumber.Id != record.Id)
            {
                throw new InvalidOperationException($"'{record.SerialNumber}' seri numarası başka bir kayıtta zaten kullanılıyor.");
            }

            bool IsSuccessed = record.Voltage >= 23.5 && record.Voltage <= 24.5;
            record.IsPassed = IsSuccessed;

            await _repository.UpdateAsync(record);

            var current = _store.Records.FirstOrDefault(x => x.Id == record.Id);
            if (current != null)
            {
                var index = _store.Records.IndexOf(current);
                _store.Records[index] = record;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);

            var record = _store.Records.FirstOrDefault(x => x.Id == id);
            if (record != null)
            _store.Records.Remove(record);
        }

    }
}
