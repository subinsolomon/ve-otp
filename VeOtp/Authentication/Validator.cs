using System;
using System.Linq;

namespace Ve.Otp.Authentication
{
    public class Validator
    {
        private Generator Generator { get; }

        public Validator()
        {
            Generator = new Generator();
        }

        public bool validateUserIdWithOtp(string userId, string otp)
        {
            var counter = Generator.CurrentT;
            var validOtps = Enumerable.Range(0, 2).Select(i => Generator.generate(userId, counter - i));
            return validOtps.Any(o => o == otp);
        }
    }
}
