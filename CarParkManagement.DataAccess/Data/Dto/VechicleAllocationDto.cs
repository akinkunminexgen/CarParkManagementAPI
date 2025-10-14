namespace CarParkManagement.DataAccess.Data.Dto
{
    public class VechicleAllocationDto
    {
        public required string VehicleReg { get; set; }
        public required int SpaceNumber { get; set; }
        public required DateTime TimeIn { get; set; }
    }
}
