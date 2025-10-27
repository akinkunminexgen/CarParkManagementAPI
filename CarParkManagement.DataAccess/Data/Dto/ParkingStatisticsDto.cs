using CarParkManagement.DataAccess.Data.Models;

namespace CarParkManagement.DataAccess.Data.Dto
{
    public class ParkingStatisticDto
    {
        public required double AverageParkingTime { get; set; }
        public required string MostCommonVehicleSize { get; set; }
        public required double TotalRevenueEarned { get; set; }

    }
}
