using CarParkManagement.DataAccess.Data.Services;

namespace CarParkManagement.DataAccess.Data.Services
{
    public class TimeCalculation : ITimeCalculation
    {
        public (int TotalMinutes, int FiveMinuteInterval, DateTime TimeOut) CalculateParkingDuration(DateTime timeIn)
        {
            DateTime now = DateTime.Now;
            TimeSpan duration = now - timeIn;

            int totalMins = (int)duration.TotalMinutes;
            int fiveMinInterval = totalMins / 5;

            return (totalMins, fiveMinInterval, now);
        }

        public decimal CalculateIncurredCharges(int totalMins, int minsInterval, decimal rpm, decimal rp5m)
        {
            decimal amount = (totalMins * rpm) + (minsInterval * rp5m);

            return amount;
        }
    }
}
