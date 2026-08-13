using MaMoScope_.Core;
using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaMoScope_.Services
{
    public partial class MotorDriversService : IMotorDriversService
    {
        private readonly HttpClient _httpClient;
        private readonly MotorDriversStore _store;

        private static readonly Regex SeriNumarasiFormati = new Regex(@"^OPT-DRV-\d{4}$", RegexOptions.Compiled);
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MotorDriversService(HttpClient httpClient, MotorDriversStore store)
        {
            _httpClient = httpClient;
            _store = store;
        }

        private void VerifySerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber) || !SeriNumarasiFormati.IsMatch(serialNumber))
            {
                throw new InvalidOperationException("Seri numarası 'OPT-DRV-XXXX' formatında olmalıdır (örnek: OPT-DRV-4521).");
            }
        }

        public (string SerialNumber, double Voltage) SimulateTestData()
        {
            var rnd = new Random();
            int randomNumber = rnd.Next(1000, 9999);
            string serialNumber = $"OPT-DRV-{randomNumber}";

            double newVoltage = 20.0 + (rnd.NextDouble() * 6.0);
            newVoltage = Math.Round(newVoltage, 1);

            return (serialNumber, newVoltage);
        }

        public async Task<(bool IsSuccessed, string TestResult)> TestandRecordVoltage(string SerialNumber, double Voltage)
        {
            VerifySerialNumber(SerialNumber);

            bool exists = _store.Records.Any(x => x.SerialNumber == SerialNumber);
            if (exists)
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
                TestDate = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync("api/motortests", newRecord);
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<MotorDrivers>(_jsonOptions);

            _store.Records.Insert(0, created ?? newRecord);
            return (IsSuccessed, TestResult);
        }

        public async Task<List<MotorDrivers>> GetPastRecordAsync()
        {
            var response = await _httpClient.GetAsync("api/motortests");
            response.EnsureSuccessStatusCode();

            var records = await response.Content.ReadFromJsonAsync<List<MotorDrivers>>(_jsonOptions);
            return records ?? new List<MotorDrivers>();
        }

        public async Task LoadAsync()
        {
            var list = await GetPastRecordAsync();

            _store.Records.Clear();
            foreach (var record in list)
                _store.Records.Add(record);
        }

        public async Task UpdateAsync(MotorDrivers record)
        {
            VerifySerialNumber(record.SerialNumber);

            var sameSerialNumber = _store.Records
                .FirstOrDefault(x => x.SerialNumber == record.SerialNumber && x.Id != record.Id);
            if (sameSerialNumber != null)
            {
                throw new InvalidOperationException($"'{record.SerialNumber}' seri numarası başka bir kayıtta zaten kullanılıyor.");
            }

            bool IsSuccessed = record.Voltage >= 23.5 && record.Voltage <= 24.5;
            record.IsPassed = IsSuccessed;

            var response = await _httpClient.PutAsJsonAsync($"api/motortests/{record.Id}", record);
            response.EnsureSuccessStatusCode();

            var current = _store.Records.FirstOrDefault(x => x.Id == record.Id);
            if (current != null)
            {
                var index = _store.Records.IndexOf(current);
                _store.Records[index] = record;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/motortests/{id}");
            response.EnsureSuccessStatusCode();

            var record = _store.Records.FirstOrDefault(x => x.Id == id);
            if (record != null)
                _store.Records.Remove(record);
        }
    }
}