using Division42LLC.WebCA.x509;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Division42LLC.WebCA.Extensions;

namespace Division42LLC.WebCA.Models
{
    public class CertificateAndPems
    {
        public CertificateAndPems(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            Certificate = certificate;

            var keyPair = DotNetUtilities.GetKeyPair(certificate.GetRSAPrivateKey() as AsymmetricAlgorithm);


            PrivateKeyPem = keyPair.ToPrivateKeyPem();
            PublicKeyPem = keyPair.ToPublicKeyPem();
        }
        

        public X509Certificate2 Certificate { get; private set; }

        public String PublicKeyPem { get; private set; }

        public String PrivateKeyPem { get; private set; }
        
    }
}
