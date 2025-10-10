namespace CarParkManagement.DataAccess.Data.Dto
{
    public class VechicleAllocationDto
    {
        public string? VehicleReg { get; set; }
        public int? SpaceNumber { get; set; }
        public DateTime? TimeIn { get; set; }
        public string? message {  get; set; }
    }
}
