using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Division42LLC.WebCA.x509
{
    public class CryptoApiRandomGenerator : IRandomGenerator
    {
        public CryptoApiRandomGenerator()
            : this(true)
        {
        }

        public CryptoApiRandomGenerator(Boolean initializeRandomizer)
            : this(new Byte[0])
        {
            InitializeRandom();
        }

        public CryptoApiRandomGenerator(Int64 seed)
            : this(Encoding.UTF8.GetBytes(seed.ToString()))
        {
        }

        public CryptoApiRandomGenerator(Byte[] seed)
        {
            _seed = seed;
        }

        public void InitializeRandom()
        {
            StringBuilder randomness = new StringBuilder();

            Int32 wait = new Random().Next(0, 500);
            randomness.Append(Environment.TickCount.ToString());
            randomness.Append(new Random().Next(1000000, Int32.MaxValue));
            Thread.Sleep(wait);
            randomness.Append(DateTime.Now.Ticks.ToString());
            randomness.Append(new Random().Next(1000000, Int32.MaxValue));

            String randomnessString = randomness.ToString();

            Byte[] randomnessBytes = Encoding.UTF8.GetBytes(randomnessString);

            AddSeedMaterial(randomnessBytes);
        }

        public void AddSeedMaterial(byte[] seed)
        {
            Int32 originalLength = _seed.Length;
            Int32 newSize = _seed.Length + seed.Length;
            Array.Resize(ref _seed, newSize);

            seed.CopyTo(_seed, originalLength);
        }

        public void AddSeedMaterial(long seed)
        {
            Byte[] seedBytes = Encoding.UTF32.GetBytes(seed.ToString());
            AddSeedMaterial(seedBytes);
        }

        public void NextBytes(byte[] bytes)
        {
            NextBytes(bytes, 0, bytes.Length);
        }


        public void NextBytes(byte[] data, Int32 start, Int32 length)
        {
            StringBuilder passwordBuilder = new StringBuilder(Convert.ToBase64String(_seed));

            for (Int32 index = 0; index < 10; index++)
            {
                passwordBuilder.Append(GetHash(passwordBuilder.ToString()));
            }

            String password = GetHash(passwordBuilder.ToString());
            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, 2048))
            {
                deriveBytes.IterationCount = 10;
                Byte[] bytes = deriveBytes.GetBytes(length);

                bytes.CopyTo(data, start);
            }
        }

        protected String GetHash(String inputText)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);

            return GetHash(inputBytes);
        }
        protected String GetHash(Byte[] inputBytes)
        {
            using (SHA512 sha = SHA512.Create())
            {
                Byte[] outputBytes = sha.ComputeHash(inputBytes);

                return Convert.ToBase64String(outputBytes);
            }
        }

        private Byte[] _seed = new Byte[0];
    }
}
