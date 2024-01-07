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
        public static string ExtractTeamCode(this string? url)
        {
            // Check if the URL contains "/teams/" and extract the team code
            int lastIndexOfSpearator = url.Trim('/').LastIndexOf("/", StringComparison.OrdinalIgnoreCase);

            if (lastIndexOfSpearator != -1)
            {
                return url.Substring(lastIndexOfSpearator + 1).Trim('/');
            }

            return null; // Return null if the team code cannot be extracted
        }
        public static TimeSpan? GetTimeSpanSum(this IEnumerable<TimeSpan> sourceList)
        {
            double doubleAverageTicks = sourceList.Sum(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            return new TimeSpan(longAverageTicks);
        }
    }
}
