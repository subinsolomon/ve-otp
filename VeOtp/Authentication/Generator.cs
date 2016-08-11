using System;
using System.Net;
using System.Security.Cryptography;

namespace Ve.Otp.Authentication
{
    public class Generator
    {
        private HMACSHA1 Hash { get; }
        private DateTime T0 { get; } = new DateTime(1970, 1, 1);

        private const int OtpLength = 6;

        public Generator()
        {
            const string SecretKey = "this isn't massivelysecret"; // Todo: Make secret.
            Hash = new HMACSHA1(EncodeString(SecretKey));
        }

        public string generate(string userId)
        {
            return generate(userId, CurrentT);
        }

        public string generate(string userId, long counter)
        {
            var key = Hash.ComputeHash(EncodeString(userId));
            return rfc6238(key, counter);
        }

        private int Interval { get; } = 30;

        public long CurrentT => (long)DateTime.UtcNow.Subtract(T0).TotalSeconds / Interval;

        private string rfc6238(byte[] K, long C)
        {
            byte[] H = (
                new HMACSHA1(K)
                .ComputeHash(
                    BitConverter.GetBytes(
                        IPAddress.HostToNetworkOrder(
                            C
                        ))));
            int O = H[H.Length - 1] & 0xf;
            int dbc1 = (H[O] << 24) | (H[O + 1] << 16) | (H[O + 2] << 8) | (H[O + 3]);
            int dbc2 = dbc1 & 0x7fffffff;
            int dc = dbc2 % (int)Math.Pow(10, Interval);
            string pdc = dc.ToString().PadLeft(OtpLength, '0');
            return pdc.Substring(pdc.Length - OtpLength);
        }

        private static byte[] EncodeString(string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);  
        }
    }
}
