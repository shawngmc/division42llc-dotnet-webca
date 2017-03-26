using Division42LLC.WebCA.x509;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Division42LLC.WebCA.Tests.x509
{
    [TestClass]
    public class CryptoApiRandomGeneratorTests
    {
        [TestMethod]
        public void VerifyRandomness()
        {
            CryptoApiRandomGenerator instance = new CryptoApiRandomGenerator(true);

            Byte[] randomBytes = new Byte[32];

            Stopwatch stopwatch = Stopwatch.StartNew();
            Int32 iterations = 2000;

            for (Int32 index = 0; index < iterations; index++)
            {
                instance.NextBytes(randomBytes);
                String output = BitConverter.ToString(randomBytes);
                String outputAsBase64 = Convert.ToBase64String(randomBytes);

                Debug.WriteLine(output + " - " + outputAsBase64);

                //System.IO.File.AppendAllText(@"C:\Data\out.txt", output + " - " + outputAsBase64 + Environment.NewLine);

                if (index % (iterations / 10) == 0)
                    Debug.WriteLine($"{index}/{iterations}");
            }

            stopwatch.Stop();

            Double perSecond = (Double)iterations / (Double)stopwatch.Elapsed.TotalSeconds;
            Double averageTime = (Double)stopwatch.Elapsed.TotalSeconds / (Double)iterations;
            Debug.WriteLine($"Elapsed: {stopwatch.Elapsed.ToString()} for {iterations.ToString()} iterations. ({perSecond:N1} iterations/second | Avg Time: {averageTime:N4})");
            
            Debugger.Break();
        }
    }
}
