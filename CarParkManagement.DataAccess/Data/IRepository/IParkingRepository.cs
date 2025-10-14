using CarParkManagement.DataAccess.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkManagement.DataAccess.Data.IRepository
{
    public interface IParkingRepository
    {
        Task<int> GetAvailableSpaceCountAsync();
        Task<int> GetOccupiedSpaceCountAsync();
        Task<ParkingSpace> GetFirstAvailableSpaceAsync();
        Task<ChargeRate?> GetChargeRateAsync(string size);
        Task<Vehicle?> GetVehicleByRegAsync(string reg);
        Task<ParkingAllocation?> GetActiveParkingAllocationAsync(int vehicleId);
        Task AddVehicleAsync(Vehicle vehicle);
        Task AddParkingAllocationAsync(ParkingAllocation parkingAllocation);
        Task SaveChangesAsync();

    }
}
