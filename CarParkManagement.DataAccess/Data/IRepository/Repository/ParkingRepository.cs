using CarParkManagement.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CarParkManagement.DataAccess.Data.IRepository.Repository
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly MyDbConnection _db;

        public ParkingRepository( MyDbConnection db)
        {
            _db = db;
        }
        public async Task AddParkingAllocationAsync(ParkingAllocation parkingAllocation)
        {
            _db.ParkingAllocations.Add(parkingAllocation);
            await _db.SaveChangesAsync();
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetAvailableSpaceCountAsync()
        {
            return await _db.ParkingSpaces.Where(p => !p.IsOccupied).CountAsync();
            
        }

        public async Task<ChargeRate?> GetChargeRateAsync(string size)
        {
           return await _db.ChargeRates.Where(c => c.Size == size).FirstOrDefaultAsync();
        }

        public async Task<ParkingSpace> GetFirstAvailableSpaceAsync()
        {
            return await _db.ParkingSpaces.Where(p => p.IsOccupied == false).FirstAsync();
        }

        public async Task<int> GetOccupiedSpaceCountAsync()
        {
            return await _db.ParkingSpaces.Where(p => p.IsOccupied == true).CountAsync();
        }

        public async Task<ParkingAllocation?> GetActiveParkingAllocationAsync(int vehicleId)
        {
            return await _db.ParkingAllocations.Where(p => p.VehicleId == vehicleId && p.IsAvailable == true)
                                                .Include(p => p.ParkingSpace)
                                                .OrderByDescending(c => c.ParkingAllocationId)
                                                .FirstOrDefaultAsync();
        }

        public async Task<Vehicle?> GetVehicleByRegAsync(string reg)
        {
            return await _db.Vehicles.Include(v => v.ChargeRate).FirstOrDefaultAsync(v => v.VehicleReg == reg);
        }

        public async Task<List<ParkingAllocation>> GetParkingHistoryAsync(int vehicleId)
        {
            return await _db.ParkingAllocations.Where(v => v.VehicleId == vehicleId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
