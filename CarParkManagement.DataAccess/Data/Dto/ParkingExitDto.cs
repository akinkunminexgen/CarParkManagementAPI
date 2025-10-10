using CarParkManagement.DataAccess.Data.Models;

namespace CarParkManagement.DataAccess.Data.Dto
{
    public class ParkingExitDto
    {
        public string? VehicleReg { get; set; }
        public double? VehicleCharge { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string? messages { get; set; }
    }

    public class ParkingExitValuesDto
    {
        public required string VehicleReg { get; set; }
        public required decimal RatePerMinute { get; set; }
        public required decimal RatePer5Minute { get; set; }
        //public required DateTime TimeIn { get; set; }
        public required ParkingAllocation ParkAllocation { get; set; }
        
    }

    public class ParkingVehicleRegDto
    {  
        public required string VehicleReg { get; set; }
    
    }
}
