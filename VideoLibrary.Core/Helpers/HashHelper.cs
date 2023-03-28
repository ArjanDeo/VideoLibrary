using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace VideoLibrary.Core.Helpers
{
    public static class HashHelper
    {
        public static string ASPIdentityHashV3(string PlainPassword)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[128 / 8];
                rng.GetBytes(salt); //The GetMethod fills the salt array with random data

                byte[] pbkdf2Hash = KeyDerivation.Pbkdf2(password: PlainPassword,
                                    salt: salt,
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 10000,
                                    numBytesRequested: 32);

                byte[] identityV3Hash = new byte[1 + 4/*KeyDerivationPrf value*/ + 4/*Iteration count*/ + 4/*salt size*/ + 16 /*salt*/ + 32 /*password hash size*/];

                identityV3Hash[0] = 1;

                uint prf = (uint)KeyDerivationPrf.HMACSHA256; // or just 1
                byte[] prfAsByteArray = BitConverter.GetBytes(prf).Reverse().ToArray(); //you need System.Linq for this to work
                Buffer.BlockCopy(prfAsByteArray, 0, identityV3Hash, 1, 4);

                byte[] iterationCountAsByteArray = BitConverter.GetBytes((uint)10000).Reverse().ToArray();
                Buffer.BlockCopy(iterationCountAsByteArray, 0, identityV3Hash, 1 + 4, 4);

                byte[] saltSizeInByteArray = BitConverter.GetBytes((uint)16).Reverse().ToArray();
                Buffer.BlockCopy(saltSizeInByteArray, 0, identityV3Hash, 1 + 4 + 4, 4);

                Buffer.BlockCopy(salt, 0, identityV3Hash, 1 + 4 + 4 + 4, salt.Length);

                Buffer.BlockCopy(pbkdf2Hash, 0, identityV3Hash, 1 + 4 + 4 + 4 + salt.Length, pbkdf2Hash.Length);

                string identityV3Base64Hash = Convert.ToBase64String(identityV3Hash);

                return identityV3Base64Hash;
            }
        }
        public static string ASPIdentityHashV2(string PlainPassword)
        {
            byte[] salt;
            byte[] buffer2;
            if (PlainPassword == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(PlainPassword, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static string GenerateSecureRandomString()
        {
            using RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);

            return Convert.ToBase64String(tokenData);
        }
    }
}
