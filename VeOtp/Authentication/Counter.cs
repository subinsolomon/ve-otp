using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ve.Otp.Authentication
{
    internal static class Counter
    {
        public static long Current =>
            (long)DateTime.UtcNow.Subtract(Epoch).TotalSeconds / IntervalInSeconds;

        public static IEnumerable<long> ValidCounters
        {
            get
            {
                var current = Current;
                return
                    Enumerable
                        .Range(0, MinimumExpiryInSeconds / IntervalInSeconds + 1)
                        .Select(i => Current - i);
            }
        }


        internal static int IntervalInSeconds { get; } = 10;

        internal static DateTime Epoch { get; } = new DateTime(1970, 1, 1);

        internal static int MinimumExpiryInSeconds { get; } = 30;
    }
}
