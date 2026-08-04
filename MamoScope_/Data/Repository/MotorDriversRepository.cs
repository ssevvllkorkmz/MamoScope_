using MaMoScope_.Core.Interface;
using MaMoScope_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Data.Repository
{
        public class MotorDriversRepository : IMotorDriversRepository
        {
            private readonly IDbContextFactory<AppDbContext> _dbFactory;

            public MotorDriversRepository(IDbContextFactory<AppDbContext> dbFactory)
            {
                _dbFactory = dbFactory;
            }

            public async Task<List<MotorDrivers>> GetAllAsync()
            {
                using var db = _dbFactory.CreateDbContext();
                return await db.MotorDrivers
                    .OrderByDescending(x => x.TestDate)
                    .ToListAsync();
            }

            public async Task AddAsync(MotorDrivers record)
            {
                using var db = _dbFactory.CreateDbContext();
                db.MotorDrivers.Add(record);
                await db.SaveChangesAsync();
            }

            public async Task<MotorDrivers?> GetBySerialNumberAsync(string serialNumber)
            {
                using var db = _dbFactory.CreateDbContext();
                return await db.MotorDrivers
                    .FirstOrDefaultAsync(x => x.SerialNumber == serialNumber);
            }

            public async Task UpdateAsync(MotorDrivers record)
            {
                using var db = _dbFactory.CreateDbContext();
                db.MotorDrivers.Update(record);
                await db.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                using var db = _dbFactory.CreateDbContext();

                var record = await db.MotorDrivers.FindAsync(id);
                if (record == null)
                    throw new InvalidOperationException($"Id={id} olan kayıt bulunamadı.");

                db.MotorDrivers.Remove(record);
                await db.SaveChangesAsync();
            }

        }
    }

