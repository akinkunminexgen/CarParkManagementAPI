using CarParkManagement.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task AddParkingAllocationAsync(ParkingAllocation parkingAllocation)
        {
            throw new NotImplementedException();
        }

        public Task AddVehicleAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetAvailableSpaceCountAsync()
        {
            int v = await _db.ParkingSpaces.Where(p => !p.IsOccupied).CountAsync();
            throw new NotImplementedException();
        }

        public Task<ChargeRate?> GetChargeRateAsync(string size)
        {
            throw new NotImplementedException();
        }

        public Task<ParkingSpace> GetFirstAvailableSpaceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOccupiedSpaceCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParkingAllocation> GetParkingAllocationAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle?> GetVehicleByRegAsync(string reg)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
