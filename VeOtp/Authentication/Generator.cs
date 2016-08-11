using System;
using System.Net;
using System.Security.Cryptography;

namespace Ve.Otp.Authentication
{
    public class Generator
    {
        public Generator()
        {
            const string SecretKey = "1234567812345678"; // Todo: Make secret.
            KeyFunction = new HMACSHA1(Convert.FromBase64String(SecretKey));
        }

        public string GenerateUserCurrentOtpFromId(string userId)
        {
            return GenerateUserOtpFromIdAndCounter(userId, Counter.Current);
        }
        public string GenerateUserOtpFromIdAndCounter(string userId, long counter)
        {
            var key = KeyFromUserId(userId);
            return TotpFromKeyAndCounter(key, counter);
        }

        private byte[] KeyFromUserId(string userId)
        {
            var encodedUserId = System.Text.Encoding.UTF8.GetBytes(userId);
            return KeyFunction.ComputeHash(encodedUserId);
        }

        /// <remarks>
        /// Currently implemented using RFC6238 TOTP.
        /// </remarks>
        private string TotpFromKeyAndCounter(byte[] key, long counter)
        {
            byte[] hash = (
                new HMACSHA1(key)
                .ComputeHash(
                    BitConverter.GetBytes(IPAddress.HostToNetworkOrder(counter))
                )
            );
            int offset = hash[hash.Length - 1] & 0xf;
            int segment = 
                ((hash[offset] << 24) | (hash[offset + 1] << 16) | (hash[offset + 2] << 8) | (hash[offset + 3]))
                & 0x7fffffff;
            int code = segment % (int)Math.Pow(10, OtpLength);
            string paddedCode = code.ToString().PadLeft(OtpLength, '0');
            return paddedCode.Substring(paddedCode.Length - OtpLength);
        }

        private HMACSHA1 KeyFunction { get; }

        private const int OtpLength = 6;
    }
}
