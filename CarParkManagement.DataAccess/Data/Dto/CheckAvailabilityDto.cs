namespace CarParkManagement.DataAccess.Data.Dto
{
    public class CheckAvailabilityDto
    {
        public int? AvailableSpaces { get; set; }
        public int? OccupiedSpaces { get; set; }
        public string? Message { get; set; }
    }


}
