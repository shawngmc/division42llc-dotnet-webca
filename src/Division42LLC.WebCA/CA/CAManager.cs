using Division42LLC.WebCA.x509;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Division42LLC.WebCA.Models;
using Division42LLC.WebCA.Extensions;
using Org.BouncyCastle.OpenSsl;

namespace Division42LLC.WebCA.CA
{
    public class CAManager
    {
        public CertificateAndPems GetCACertificate(String password = null)
        {
            if (Directory.Exists(CAStorePathInfo.CARootPath) && File.Exists(CAStorePathInfo.CACertPathAndFileName))
            {
                X509Certificate2 caCertFromFile = new X509Certificate2(CAStorePathInfo.CACertPathAndFileName, null, X509KeyStorageFlags.Exportable);

                //RSA caPrivateKeyFromFile = caCertFromFile.GetRSAPrivateKey();

                return new CertificateAndPems(caCertFromFile);
            }
            else
            {
                return null;
            }
        }

        public CertificateAndPems GetLeafCertificate(String thumbprint, String password = null)
        {
            String leafPathAndFilename = Path.Combine(CAStorePathInfo.LeafCertPath, $"{thumbprint}.pfx");
            if (Directory.Exists(CAStorePathInfo.LeafCertPath) && File.Exists(leafPathAndFilename))
            {
                X509Certificate2 certificate = new X509Certificate2(leafPathAndFilename, password, X509KeyStorageFlags.Exportable);

                return new CertificateAndPems(certificate);
            }
            else
            {
                return null;
            }
        }

        public void WipeExistingStore()
        {
            try
            {
                String[] subItems = Directory.GetFileSystemEntries(CAStorePathInfo.CARootPath);

                foreach (String subItem in subItems)
                {
                    try
                    {
                        Directory.Delete(subItem, true);
                    }
                    catch (Exception exception)
                    {
                        if (exception is UnauthorizedAccessException || exception is IOException)
                        {
                        }
                        else
                            throw;
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception is UnauthorizedAccessException || exception is IOException)
                {
                }
                else
                    throw;
            }
        }

        public void GenerateNewLeafCertificate(string name, string organization, string organizationalUnit, string city, string stateCode, string countryCode, string password)
        {
            if (!Directory.Exists(CAStorePathInfo.CACertPath))
                Directory.CreateDirectory(CAStorePathInfo.CACertPath);

            CertificateGenerator generator = new CertificateGenerator();

            String subjectDN = $"CN={name},O={organization},OU={organizationalUnit},L={city},C={countryCode}"; //,ST={stateCode}";
            String[] subjectAlternativeNames = new List<String>()
            {
                name
            }.ToArray();

            KeyPurposeID[] usages = new List<KeyPurposeID>()
            {
                //KeyPurposeID.AnyExtendedKeyUsage,
                KeyPurposeID.IdKPClientAuth,
                KeyPurposeID.IdKPEmailProtection,
                KeyPurposeID.IdKPServerAuth
            }.ToArray();

            X509Certificate2 issuerCertificate = GetCACertificate("test").Certificate;
            AsymmetricAlgorithm issuerPrivateKey = issuerCertificate.GetRSAPrivateKey();

            X509Certificate2 certForCA = generator.IssueCertificate(subjectDN, issuerCertificate, issuerPrivateKey, subjectAlternativeNames, usages).Certificate;


            try
            {
                if (!Directory.Exists(CAStorePathInfo.LeafCertPath))
                    Directory.CreateDirectory(CAStorePathInfo.LeafCertPath);

                String leafPathAndFilename = Path.Combine(CAStorePathInfo.LeafCertPath, $"{certForCA.Thumbprint}.pfx");

                File.WriteAllBytes(leafPathAndFilename, certForCA.Export(X509ContentType.Pfx, password));
                Console.WriteLine("PFX/PKCS12: SUCCESS");
            }
            catch (Exception exception)
            {
                Console.WriteLine("PFX/PKCS12: " + exception.Message);
            }
        }

        public IEnumerable<CertificateEntry> GetAllLeafCertificates()
        {
            //String leafPathAndFilename = Path.Combine(CAStorePathInfo.LeafCertPath, $"{thumbprint}.pfx");
            if (Directory.Exists(CAStorePathInfo.LeafCertPath))
            {
                IEnumerable<String> leafCertificateFiles = Directory.EnumerateFiles(CAStorePathInfo.LeafCertPath, "*.pfx");
                foreach (String leafCertificateFile in leafCertificateFiles)
                {
                    X509Certificate2 certificate = new X509Certificate2(leafCertificateFile, null);

                    yield return new CertificateEntry()
                    {
                        DistinguishedName = certificate.Subject,
                        ExpiresOn = certificate.NotAfter,
                        IssuedOn = certificate.NotBefore,
                        SerialNumber = certificate.SerialNumber,
                        Thumbprint = certificate.Thumbprint,
                        PublicKey = certificate.ExportToPEM(),
                        DistinguishedNameDetails = certificate.ExtractDNFields(),
                        KeySize = $"{certificate.GetRSAPublicKey().KeySize}-bit"
                    };
                }
            }
            else
            {
                yield return null;
            }
        }

        public void GenerateNewCACertificate(String name, String organization = "AutoCA",
            String organizationalUnit = "IT Security", String city = "Tampa",
            String stateCode = "FL", String countryCode = "US", String privateKeyPassword = null)
        {

            if (!Directory.Exists(CAStorePathInfo.CACertPath))
                Directory.CreateDirectory(CAStorePathInfo.CACertPath);

            CertificateGenerator generator = new CertificateGenerator();

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
            String[] subjectAlternativeNames =
                new List<String>().ToArray();

            // NO usages for a CA cert.
            KeyPurposeID[] usages = new List<KeyPurposeID>().ToArray();

            X509Certificate2 certForCA = generator.CreateCertificateAuthorityCertificate(subjectDN, subjectAlternativeNames, usages).Certificate;

            //try
            //{
            //    File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName, certForCA.Export(X509ContentType.SerializedCert, privateKeyPassword));
            //    Console.WriteLine("Serialized: SUCCESS");
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("Serialized: " + exception.Message);
            //}

            //try
            //{
            //    File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName + ".auth", certForCA.Export(X509ContentType.Authenticode, privateKeyPassword));
            //    Console.WriteLine("Authenticode: SUCCESS");
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("Authenticode: " + exception.Message);
            //}

            //try
            //{
            //    File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName + ".crt", certForCA.Export(X509ContentType.Cert, privateKeyPassword));
            //    Console.WriteLine("Cert: SUCCESS");
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("Cert: " + exception.Message);
            //}

            try
            {
                File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName, certForCA.Export(X509ContentType.Pfx, privateKeyPassword));
                Console.WriteLine("PFX/PKCS12: SUCCESS");
            }
            catch (Exception exception)
            {
                Console.WriteLine("PFX/PKCS12: " + exception.Message);
            }

            //try
            //{
            //    File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName + ".p7b", certForCA.Export(X509ContentType.Pkcs7, privateKeyPassword));
            //    Console.WriteLine("P7B: SUCCESS");
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("P7B: " + exception.Message);
            //}
        }

        public String GetCARootFolder()
        {
            try
            {
                String caRoot = Environment.GetEnvironmentVariable("CA_ROOT");

                if (String.IsNullOrWhiteSpace(caRoot))
                    throw new CAConfigurationException($"While attempting to determine the root folder for the Certificate Authority, it was found the environment variable \"CA_ROOT\" was null or empty. This should point to the folder which represents where the CA files are stored.");

                return caRoot;
            }
            catch (SecurityException exception)
            {
                throw new CAConfigurationException($"An exception of type \"{exception.GetType().ToString()}\" occurred while attempting to determine the root folder for the Certificate Authority. The environment variable \"CA_ROOT\" should point to the folder which represents where the CA files are stored.", exception);
            }
        }

        private void test()
        {
            //PemWriter



        }
    }
}
