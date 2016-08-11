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

        public bool ValidateUserFromIdUsingOtp(string userId, string otp)
        {
            var counter = Generator.CurrentCounter;
            var validOtps = Enumerable.Range(0, 2).Select(i => Generator.GenerateUserOtpFromIdAndCounter(userId, counter - i));
            return validOtps.Any(o => o == otp);
        }
    }
}
