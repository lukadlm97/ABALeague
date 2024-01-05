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
            int teamsIndex = url.IndexOf("/teams/", StringComparison.OrdinalIgnoreCase);

            if (teamsIndex != -1)
            {
                int startIndex = teamsIndex + "/teams/".Length;
                int endIndex = url.IndexOf("/", startIndex, StringComparison.OrdinalIgnoreCase);

                if (endIndex != -1)
                {
                    return url.Substring(startIndex, endIndex - startIndex);
                }
            }

            return null; // Return null if the team code cannot be extracted
        }
    }
}
