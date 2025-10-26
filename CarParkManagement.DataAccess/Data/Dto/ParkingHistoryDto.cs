using CarParkManagement.DataAccess.Data.Models;

namespace CarParkManagement.DataAccess.Data.Dto
{
    public class ParkingHistoryDto
    {
        public required string VehicleReg { get; set; }
        public required DateTime TimeIn { get; set; }
        public required DateTime? TimeOut { get; set; }
        public decimal? Charge { get; set; }
    }
}
