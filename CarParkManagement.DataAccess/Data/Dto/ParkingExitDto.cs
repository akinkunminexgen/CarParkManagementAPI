using CarParkManagement.DataAccess.Data.Models;

namespace CarParkManagement.DataAccess.Data.Dto
{
    public class ParkingExitDto
    {
        public required string VehicleReg { get; set; }
        public required double VehicleCharge { get; set; }
        public required DateTime TimeIn { get; set; }
        public required DateTime TimeOut { get; set; }
    }


    public class ParkingVehicleRegDto
    {  
        public required string VehicleReg { get; set; }
    
    }
}
