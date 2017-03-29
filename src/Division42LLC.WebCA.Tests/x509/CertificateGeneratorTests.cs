using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Division42LLC.WebCA.x509;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Cryptography;
using System.IO;

namespace Division42LLC.WebCA.Tests.x509
{
    [TestClass]
    public class CertificateGeneratorTests
    {

        [TestMethod]
        public void GenerateSelfSignedCertificateWithValidArgs_ReturnsExpected()
        {

            String name = "unit-tester";
            String organization = "Division42 LLC";
            String organizationalUnit = "IT Security";
            String city = "Tampa";
            String stateCode = "FL";
            String countryCode = "US";


            CertificateGenerator instance = new CertificateGenerator();

            //DC domainComponent
            //CN commonName
            //OU organizationalUnitName
            //O organizationName
            //STREET streetAddress
            //L localityName
            //ST stateOrProvinceName
            //C countryName
            //UID userid

            String subjectDN = $"CN={name},O={organization},OU={organizationalUnit},L={city},C={countryCode}"; //,ST={stateCode}";
            String[] subjectAlternativeNames = new List<String>().ToArray();
            KeyPurposeID[] usages = new List<KeyPurposeID>() { KeyPurposeID.AnyExtendedKeyUsage }.ToArray();

            TimeIt("Create CA", () =>
            {
                // CA
                {
                    X509Certificate2 certForCA = instance.CreateCertificateAuthorityCertificate(subjectDN, subjectAlternativeNames, usages).Certificate;
                    
                    File.WriteAllBytes(@"C:\Data\cert-CA.sst", certForCA.Export(X509ContentType.SerializedCert, "test"));
                }
            });

            X509Certificate2 caCertFromFile = null;
            RSA caPrivateKeyFromFile = null;

            TimeIt("Get CA from file system", () =>
            {
                caCertFromFile = new X509Certificate2(@"C:\Data\cert-CA.pfx", "test");
                caPrivateKeyFromFile = caCertFromFile.GetRSAPrivateKey();
            });

            TimeIt("Generate Leaf1", () =>
            {
                // LEAF1
                String dnForLeaf1 = $"CN=leaf1,O={organization},OU={organizationalUnit},L={city},C={countryCode}";
                X509Certificate2 certForLeaf1 = instance.IssueCertificate(dnForLeaf1, caCertFromFile, caPrivateKeyFromFile, subjectAlternativeNames, usages).Certificate;
                File.WriteAllBytes(@"C:\Data\cert-leaf1.pfx", certForLeaf1.Export(X509ContentType.Pkcs12, "test"));
            });

            TimeIt("Generate Leaf1", () =>
            {
                // LEAF2
                String dnForLeaf2 = $"CN=leaf2,O={organization},OU={organizationalUnit},L={city},C={countryCode}";
                X509Certificate2 certForLeaf2 = instance.IssueCertificate(dnForLeaf2, caCertFromFile, caPrivateKeyFromFile, subjectAlternativeNames, usages).Certificate;
                File.WriteAllBytes(@"C:\Data\cert-leaf2.pfx", certForLeaf2.Export(X509ContentType.Pkcs12, "test"));
            });

            TimeIt("Generate Leaf1", () =>
            {
                // LEAF2
                String dnForLeaf3 = $"CN=leaf3,O={organization},OU={organizationalUnit},L={city},C={countryCode}";
                X509Certificate2 certForLeaf3 = instance.IssueCertificate(dnForLeaf3, caCertFromFile, caPrivateKeyFromFile, subjectAlternativeNames, usages).Certificate;
                File.WriteAllBytes(@"C:\Data\cert-leaf3.pfx", certForLeaf3.Export(X509ContentType.Pkcs12, "test"));
            });

            Debugger.Break();
        }


        private void TimeIt(String title, Action code)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            code();

            stopwatch.Stop();

            Debug.WriteLine($"{title} - {stopwatch.Elapsed.ToString()}");
        }
    }
}
