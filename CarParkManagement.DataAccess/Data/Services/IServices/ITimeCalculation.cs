namespace CarParkManagement.DataAccess.Data.Services
{
    public interface ITimeCalculation
    {
        (int TotalMinutes, int FiveMinuteInterval, DateTime TimeOut) CalculateParkingDuration(DateTime timeIn);
        decimal CalculateIncurredCharges(int totalMins, int minsInterval, decimal rpm, decimal rp5m);
    }
}
