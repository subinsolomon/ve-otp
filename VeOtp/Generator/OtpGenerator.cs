using System;
using System.Security.Cryptography;

namespace Ve.Otp.Generator
{
    public class OtpGenerator
    {
        private HMACSHA1 Hash { get; }

        private const int OtpLength = 6;

        public OtpGenerator()
        {
            const string SecretKey = "thisisntmassivelysecret="; // Todo: Make secret.
            Hash = new HMACSHA1(Convert.FromBase64String(SecretKey));
        }

        public string generate(string userId)
        {
            var fullOtp = Hash.ComputeHash(Convert.FromBase64String(userId));
            var shortOtp = Convert.ToBase64String(fullOtp).Substring(0, OtpLength);
            return shortOtp;
        }
    }
}
