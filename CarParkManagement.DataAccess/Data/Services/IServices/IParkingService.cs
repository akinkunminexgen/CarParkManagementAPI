using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarParkManagement.DataAccess.Data.Dto;

namespace CarParkManagement.DataAccess.Data.Services.IServices
{
    public interface IParkingService
    {
        Task<CheckAvailabilityDto> GetAvailableSpace();
        Task<VechicleAllocationDto> OccupySpace(OccupySpaceDto request);
        Task<ParkingExitDto> ToexitAway(ParkingVehicleRegDto request);

    }
}
