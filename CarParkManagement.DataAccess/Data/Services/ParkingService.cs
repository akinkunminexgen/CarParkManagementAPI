using CarParkManagement.DataAccess.Data.Dto;
using CarParkManagement.DataAccess.Data.Enums;
using CarParkManagement.DataAccess.Data.IRepository;
using CarParkManagement.DataAccess.Data.Models;
using CarParkManagement.DataAccess.Data.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkManagement.DataAccess.Data.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _repo;
        private readonly ITimeCalculation _timeCalculation;
        public ParkingService(IParkingRepository repo, ITimeCalculation timeCalculation) { 
            _repo = repo;
            _timeCalculation = timeCalculation;
        }
        public async Task<CheckAvailabilityDto> GetAvailableSpace()
        {
            var availableSpaces = await _repo.GetAvailableSpaceCountAsync();
            var occupiedSpaces = await _repo.GetOccupiedSpaceCountAsync();

            return new CheckAvailabilityDto
            {
                AvailableSpaces = availableSpaces,
                OccupiedSpaces = occupiedSpaces
            };
        }

        public async Task<VechicleAllocationDto> OccupySpace(OccupySpaceDto request)
        {
            bool toCheck = false;

            //to ensure specific type of vehicles are accepted
            if (!Enum.TryParse<VehicleType>(request.VehicleType, true, out var vehicleTypeEnum))
            {
                throw new ArgumentException($"Invalid vehicle type. Allowed types: {string.Join(", ", Enum.GetNames(typeof(VehicleType)))}");
            }

            //categorizing the vehicle types
            Size size = vehicleTypeEnum switch
            {
                VehicleType.Saloon => Size.Small,
                VehicleType.Motorcycle => Size.Small,
                VehicleType.SUV => Size.Medium,
                VehicleType.Van => Size.Medium,
                VehicleType.Truck => Size.Large,
                _ => Size.Small
            };
            string siz = size.ToString();

            ChargeRate chargeRate = await _repo.GetChargeRateAsync(siz) ?? throw new InvalidOperationException("Charge Rate not found");
            ParkingSpace parkingSpace = await _repo.GetFirstAvailableSpaceAsync() ?? throw new InvalidOperationException("No available parking space");

            Vehicle vehicle = await _repo.GetVehicleByRegAsync(request.VehicleReg)
                    ?? new Vehicle
                    {
                        VehicleReg = request.VehicleReg,
                        VehicleType = vehicleTypeEnum.ToString(),
                        ChargeRateId = chargeRate.ChargeRateId,
                    };
            if (vehicle.VehicleId == 0)
            {
                 await _repo.AddVehicleAsync(vehicle);
            }

            ParkingAllocation parkingAllocation = await _repo.GetActiveParkingAllocationAsync(vehicle.VehicleId) ??
                new ParkingAllocation
                {
                    VehicleId = vehicle.VehicleId,
                    ParkingSpaceId = parkingSpace.ParkingSpaceId,
                    TimeIn = DateTime.Now,
                    IsAvailable = true,
                };

            if (parkingAllocation.ParkingAllocationId == 0)
            {
                //to ensure parkSpace is not ocuppied if it is still the same car
                parkingSpace.IsOccupied = true;
                toCheck = true;

                await _repo.AddParkingAllocationAsync(parkingAllocation);
            }

            return new VechicleAllocationDto
            {
                VehicleReg = vehicle.VehicleReg,
                SpaceNumber = !toCheck ? parkingAllocation.ParkingSpace.SpaceNumber : parkingSpace.SpaceNumber,
                TimeIn = parkingAllocation.TimeIn,
            };

        }

        public async Task<ParkingExitDto> ToexitAway(ParkingVehicleRegDto request)
        {
            Vehicle? vehicle = await _repo.GetVehicleByRegAsync(request.VehicleReg) ?? throw new InvalidOperationException($"Vehicle `{request.VehicleReg}` not found");

            ParkingAllocation parkingAllocation = await _repo.GetActiveParkingAllocationAsync(vehicle.VehicleId) ?? throw new InvalidOperationException($"No Active parking record found for `{request.VehicleReg}`");

            decimal? chargeRate = parkingAllocation.Charge;


            if (parkingAllocation.Charge != null)
            {
                throw new InvalidOperationException($"Vehicle with registration '{request.VehicleReg}'has been charged.");
            }

            var (TotalMinutes, FiveMinuteInterval, TimeOut) = _timeCalculation.CalculateParkingDuration(parkingAllocation.TimeIn);
            double charges = (double)_timeCalculation.CalculateIncurredCharges(TotalMinutes, FiveMinuteInterval, vehicle.ChargeRate.RatePerMinute, vehicle.ChargeRate.ExtraChargePer5Min);

            parkingAllocation.Charge = (decimal)charges;
            parkingAllocation.TimeOut = TimeOut;

            //this is to ensure a vehicle can park multiple time
            parkingAllocation.IsAvailable = false;
            parkingAllocation.ParkingSpace.IsOccupied = false;

            await _repo.SaveChangesAsync();
            return new ParkingExitDto
            {
                VehicleReg = request.VehicleReg,
                VehicleCharge = charges,
                TimeIn = parkingAllocation.TimeIn,
                TimeOut = TimeOut,
            };

        }
    }
}
