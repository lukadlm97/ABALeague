using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public static class DistanceCalculator
    {
        public static int CalculateAge(this DateOnly birthDate, DateOnly currentDate)
        {
            int age = currentDate.Year - birthDate.Year;

            // Check if the birthday hasn't occurred yet this year
            if (currentDate.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            return age;
        }
    }
}
