using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Helper
{
    public class TimerService
    {
        public static bool CheckDayLarger(DateTime timeFrom, DateTime timeTo) 
        {
            if (timeFrom.Year > timeTo.Year) return true;

            if (timeFrom.Year == timeTo.Year
                && timeFrom.Month > timeTo.Month) return true;
            if (timeFrom.Year == timeTo.Year
                && timeFrom.Month == timeTo.Month
                && timeFrom.Day > timeTo.Day) return true;

            return false;
        }
    }
}
