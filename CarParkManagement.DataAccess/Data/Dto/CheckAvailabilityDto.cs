namespace CarParkManagement.DataAccess.Data.Dto
{
    public class CheckAvailabilityDto
    {
        public required int AvailableSpaces { get; set; }
        public required int OccupiedSpaces { get; set; }
    }
}
