using CarParkManagement.DataAccess.Data;
using CarParkManagement.DataAccess.Data.Models;
using CarParkManagement.DataAccess.Data.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CarParkManagement.DataAccess.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Collections.Immutable;
using CarParkManagement.DataAccess.Data.Services;

namespace CarParkManagement.Controllers
{
    [ApiController]
    [Route("parking")]
    public class ParkingController : ControllerBase
    {

        private readonly ILogger<ParkingController> _logger;
        private readonly MyDbConnection _db;
        private readonly ITimeCalculation _timeCalculation;

        public ParkingController(ILogger<ParkingController> logger, MyDbConnection db, ITimeCalculation timeCalculation)
        {
            _logger = logger;
            _db = db;
            _timeCalculation = timeCalculation;
        }

        [HttpGet]
        public async Task<IEnumerable<CheckAvailabilityDto>> GetAvailableSpace()
        {
            var avail = await _db.ParkingSpaces.Where(p => !p.IsOccupied).CountAsync();
            var occupied = await _db.ParkingSpaces.Where(p => p.IsOccupied == true).CountAsync();
            CheckAvailabilityDto response = new CheckAvailabilityDto
            {
                AvailableSpaces = avail,
                OccupiedSpaces = occupied
            };

            return [response]; //NOTE THIS CAN ALSO BE WRITTEN USING Ok. JUST MAKING USE OF THE SWAGGER
        }

        [HttpPost]
        public async Task<IEnumerable<VechicleAllocationDto>>  OccupySpace([FromBody] OccupySpaceDto request)
        {
            bool toCheck = false;
            if (!Enum.TryParse<VehicleType>(request.VehicleType, true, out var vehicleTypeEnum))
            {
                var response = new VechicleAllocationDto
                {
                    message = $"Invalid vehicle type. Allowed types: {string.Join(", ", Enum.GetNames(typeof(VehicleType)))}",
                };
                return [response];
            }

            try
            {
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
                int ChargeRateId = await _db.ChargeRates
                                        .Where(c => c.Size == siz)  
                                        .Select(c => c.ChargeRateId)
                                        .FirstOrDefaultAsync();
                ParkingSpace space = await _db.ParkingSpaces.Where(p => p.IsOccupied == false).FirstAsync();
                
                Vehicle vehicle = _db.Vehicles
                    .FirstOrDefault(v => v.VehicleReg == request.VehicleReg)
                    ?? new Vehicle
                    {
                        VehicleReg = request.VehicleReg,
                        VehicleType = vehicleTypeEnum.ToString(),
                        ChargeRateId = ChargeRateId,
                    };

                if (vehicle.VehicleId == 0)
                {
                    _db.Vehicles.Add(vehicle);
                    await _db.SaveChangesAsync();
                }
                    



                ParkingAllocation? allocation = _db.ParkingAllocations.Where(p => p.VehicleId == vehicle.VehicleId && p.IsAvailable == true)
                                                   .Include(p=> p.ParkingSpace)
                                                .OrderByDescending(c => c.ParkingAllocationId)
                                                .FirstOrDefault()
                                                ?? new ParkingAllocation
                                                {
                                                    VehicleId = vehicle.VehicleId,
                                                    ParkingSpaceId = space.ParkingSpaceId,
                                                    TimeIn = DateTime.Now,
                                                    IsAvailable = true,
                                                };
                if (allocation.ParkingAllocationId == 0)
                {
                    //to ensure parkSpace is not ocuppied if it is still the same person
                    space.IsOccupied = true;
                    toCheck = true;

                    _db.ParkingAllocations.Add(allocation);
                    await _db.SaveChangesAsync();
                }
                    
                var response = new VechicleAllocationDto
                {
                    VehicleReg = vehicle.VehicleReg,
                    SpaceNumber = !toCheck ? allocation.ParkingSpace.SpaceNumber : space.SpaceNumber,
                    TimeIn = allocation.TimeIn,
                };

                return [response];
            }
            catch (Exception ex) {
                var response = new VechicleAllocationDto
                {
                    message = ex.Message,
                };
                return [response]; //NOTE THIS CAN ALSO BE WRITTEN USING BadRequest. JUST MAKING USE OF THE SWAGGER
            }            

        }

        [HttpPost("exit")]
        public async Task<IEnumerable<ParkingExitDto>> ToexitAway([FromBody] ParkingVehicleRegDto request)
        {
            try
            {
                ParkingExitValuesDto? parkingTime = await _db.Vehicles.Where(v => v.VehicleReg == request.VehicleReg).
                Select(v => new ParkingExitValuesDto
                {
                    VehicleReg = v.VehicleReg,
                    RatePerMinute = v.ChargeRate.RatePerMinute,
                    RatePer5Minute = v.ChargeRate.ExtraChargePer5Min,
                    //TimeIn = v.ParkingAllocations.OrderByDescending(p => p.ParkingAllocationId).Select(s => s.TimeIn).FirstOrDefault(),
                    ParkAllocation = v.ParkingAllocations
                                    .OrderByDescending(p => p.ParkingAllocationId)
                                    .Select(s => new ParkingAllocation
                                    {
                                        ParkingAllocationId = s.ParkingAllocationId,
                                        TimeIn = s.TimeIn,
                                        Charge = s.Charge,
                                        TimeOut = s.TimeOut,
                                        ParkingSpace = s.ParkingSpace,
                                    }).First(),

                }).FirstOrDefaultAsync();

                if (parkingTime == null || parkingTime.ParkAllocation == null || parkingTime.ParkAllocation.Charge != null)
                {
                    ParkingExitDto resp = new ParkingExitDto
                    {
                        messages = $"Vehicle with registration '{request.VehicleReg}' has no parking record or has been charged."
                    };
                    return [resp]; //NOTE THIS CAN ALSO BE WRITTEN USING NOTFOUND. JUST MAKING USE OF THE SWAGGER
                }


                DateTime TimeIn = parkingTime.ParkAllocation.TimeIn;
                var (TotalMinutes, FiveMinuteInterval, TimeOut) = _timeCalculation.CalculateParkingDuration(TimeIn);
                double charges = (double)_timeCalculation.CalculateIncurredCharges(TotalMinutes, FiveMinuteInterval, parkingTime.RatePerMinute, parkingTime.RatePer5Minute);

                //to ensure the ef core tracks parkallocation 
                ParkingAllocation allocation = await _db.ParkingAllocations
                                    .FirstAsync(p => p.ParkingAllocationId == parkingTime.ParkAllocation.ParkingAllocationId);
                allocation.Charge = (decimal)charges;
                allocation.TimeOut = TimeOut;

                //this is to ensure a vehicle can park multiple time
                allocation.IsAvailable = false;

                parkingTime.ParkAllocation.ParkingSpace.IsOccupied = false;

                await _db.SaveChangesAsync();
                ParkingExitDto response = new ParkingExitDto
                {
                    VehicleReg = request.VehicleReg,
                    VehicleCharge = charges,
                    TimeIn = TimeIn,
                    TimeOut = TimeOut,
                };

                return [response]; //NOTE THIS CAN ALSO BE WRITTEN USING Ok. JUST MAKING USE OF THE SWAGGER
            }
            catch (Exception ex)
            {
                ParkingExitDto resp = new ParkingExitDto
                {
                    messages = ex.Message
                };
                return [resp]; //NOTE THIS CAN ALSO BE WRITTEN USING BadRequest. JUST MAKING USE OF THE SWAGGER
            }
            
        }

    }
}
