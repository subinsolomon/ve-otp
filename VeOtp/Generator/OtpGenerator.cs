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
            const string SecretKey = "this isn't massivelysecret"; // Todo: Make secret.
            Hash = new HMACSHA1(EncodeString(SecretKey));
        }

        public string generate(string userId)
        {
            var fullOtp = Hash.ComputeHash(EncodeString(userId));
            var shortOtp = Convert.ToBase64String(fullOtp).Substring(0, OtpLength);
            return shortOtp;
        }

        private static byte[] EncodeString(string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);  
        }
    }
}
