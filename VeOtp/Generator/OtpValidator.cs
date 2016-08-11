using System;

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
            var validOtp = Generator.generate(userId);
            return validOtp == otp;
        }
    }
}
