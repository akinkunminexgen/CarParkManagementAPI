using CarParkManagement.DataAccess.Data.Services;

namespace CarParkManagement.DataAccess.Data.Services
{
    public class TimeCalculation : ITimeCalculation
    {
        //this method is to break down minutes into intervals
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
            //ensure Every 5 minutes an additional charge of £1 will be added
            decimal amount = (totalMins * rpm) + (minsInterval * rp5m);

            return amount;
        }
    }
}
