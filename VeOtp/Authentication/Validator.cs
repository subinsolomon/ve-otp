using System.Linq;

namespace Ve.Otp.Authentication
{
    public class Validator
    {
        public Validator()
        {
            Generator = new Generator();
        }

        public bool ValidateUserFromIdUsingOtp(string userId, string otp)
        {
            var validOtps =
                Counter
                    .ValidCounters
                    .Select(c => Generator.GenerateUserOtpFromIdAndCounter(userId, c));
            return validOtps.Any(o => o == otp);
        }

        private Generator Generator { get; }
    }
}
