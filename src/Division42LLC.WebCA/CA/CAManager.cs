using Division42LLC.WebCA.x509;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Division42LLC.WebCA.CA
{
    public class CAManager
    {
        public X509Certificate2 GetCACertificate(String password = null)
        {
            if (Directory.Exists(CAStorePathInfo.CARootPath) && File.Exists(CAStorePathInfo.CACertPathAndFileName))
            {
                X509Certificate2 caCertFromFile = new X509Certificate2(CAStorePathInfo.CACertPathAndFileName, null);
                //RSA caPrivateKeyFromFile = caCertFromFile.GetRSAPrivateKey();

                return caCertFromFile;
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

            KeyPurposeID[] usages =
                new List<KeyPurposeID>() { KeyPurposeID.AnyExtendedKeyUsage }.ToArray();

            X509Certificate2 certForCA = generator.CreateCertificateAuthorityCertificate(subjectDN, subjectAlternativeNames, usages);

            File.WriteAllBytes(CAStorePathInfo.CACertPathAndFileName, certForCA.Export(X509ContentType.SerializedCert, privateKeyPassword));

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
    }
}
