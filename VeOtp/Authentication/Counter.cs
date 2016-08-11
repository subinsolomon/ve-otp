using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Ve.Otp.Authentication
{
    internal static class Counter
    {
        static Counter() {
            IntervalInSeconds = int.Parse(ConfigurationManager.AppSettings["uniqueOtpDurationInSeconds"] ?? "10");
            MinimumExpiryInSeconds = int.Parse(ConfigurationManager.AppSettings["minimumValidityTimeInSeconds"] ?? "30");
        }

        public static long Current =>
            (long)DateTime.UtcNow.Subtract(Epoch).TotalSeconds / IntervalInSeconds;

        public static IEnumerable<long> ValidCounters
        {
            get
            {
                var current = Current;
                return
                    Enumerable
                        .Range(0, (MinimumExpiryInSeconds / IntervalInSeconds) + 1)
                        .Select(i => Current - i);
            }
        }


        internal static int IntervalInSeconds { get; }

        internal static DateTime Epoch { get; } = new DateTime(1970, 1, 1);

        internal static int MinimumExpiryInSeconds { get; }
    }
}
