using System;
using System.Linq;

namespace Ve.Otp.Generator
{
    public class OtpValidator
    {
        private OtpGenerator Generator { get; }

        public OtpValidator()
        {
            Generator = new OtpGenerator();
        }

        public bool validateUserIdWithOtp(string userId, string otp)
        {
            var counter = Generator.CurrentT;
            var validOtps = Enumerable.Range(0, 2).Select(i => Generator.generate(userId, counter - i));
            return validOtps.Any(o => o == otp);
        }
    }
}
