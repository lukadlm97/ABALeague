using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public static class TimeSpanExstention
    {
        /// <summary>
        /// Calculates the average of the given timeSpans.
        /// </summary>
        public static TimeSpan Average(this IEnumerable<TimeSpan> timeSpans)
        {
            IEnumerable<long> ticksPerTimeSpan = timeSpans.Select(t => t.Ticks);
            double averageTicks = ticksPerTimeSpan.Average();
            long averageTicksLong = Convert.ToInt64(averageTicks);

            TimeSpan averageTimeSpan = TimeSpan.FromTicks(averageTicksLong);

            return averageTimeSpan;
        }
    }
}
