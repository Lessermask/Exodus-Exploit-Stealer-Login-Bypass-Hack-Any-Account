using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Exodus.Security
{
    public class Salted_Hash
    {
        public byte[] Hash { get; set; } = null;

        public byte[] Salt { get; set; } = null;

        public int Iterations { get; set; } = 0;
    }

    public static class Security
    {
        public static byte[] CreateToken(int TokenLength)
        {
            if (TokenLength <= 0)
            { throw new ArgumentOutOfRangeException("TokenLength", "number must be positive"); }

            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            var salt = new byte[TokenLength];
            rngCsp.GetNonZeroBytes(salt);
            return salt;
        }

        public static class HashHMACSHA1
        {
            public static int hash_lenght = 64;

            public static Salted_Hash CreateSaltedHash(string stringToHash, int iterations, int saltLenght = 64)
            {
                if (stringToHash == null)
                { throw new ArgumentNullException("password"); }
                else if (iterations <= 0)
                { throw new ArgumentOutOfRangeException("iterations", "number must be positive"); }
                else if (saltLenght <= 0)
                { throw new ArgumentOutOfRangeException("saltLenght", "number must be positive"); }

                Salted_Hash salted_Hash = new Salted_Hash()
                {
                    Salt = Security.CreateToken(saltLenght),
                    Iterations = iterations
                };

                using (Rfc2898DeriveBytes hashDriver = new Rfc2898DeriveBytes(
                    stringToHash,       //  string to hash
                    salted_Hash.Salt,   //  salt
                    iterations))        //  hash iterations
                {
                    salted_Hash.Hash = hashDriver.GetBytes(hash_lenght);
                }
                return salted_Hash;
            }

            public static bool CheckSaltedHash(string stringToCheck, Salted_Hash saltedHash)
            {
                if (stringToCheck == null)
                { throw new ArgumentNullException("password"); }
                else if (saltedHash.Hash == null)
                { throw new ArgumentNullException("salt"); }
                else if (saltedHash.Salt == null)
                { throw new ArgumentNullException("hashedPassword"); }
                else if (saltedHash.Iterations <= 0)
                { throw new ArgumentOutOfRangeException("iterations", "number must be positive"); }

                byte[] hashGenerated = null;

                using (Rfc2898DeriveBytes hashDriver = new Rfc2898DeriveBytes(
                    stringToCheck,          //  string to hash
                    saltedHash.Salt,        //  salt
                    saltedHash.Iterations)) //  hash iterations
                {
                    hashGenerated = hashDriver.GetBytes(hash_lenght);
                }
                return ByteArraysEqual(hashGenerated, saltedHash.Hash);
            }

            private static bool ByteArraysEqual(byte[] buff1, byte[] buff2)
            {
                if (buff1.Length != buff2.Length) { return false; }
                for (int i = 0; i < buff2.Length; i++)
                {
                    if (buff1[i] != buff2[i]) { return false; }
                }
                return true;
            }
        }

    }
}