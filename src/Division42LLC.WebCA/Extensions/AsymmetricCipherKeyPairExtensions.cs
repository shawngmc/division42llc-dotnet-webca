using Division42LLC.WebCA.x509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Division42LLC.WebCA.Extensions
{
    public static class AsymmetricCipherKeyPairExtensions
    {
        public static String ToPrivateKeyPem(this AsymmetricCipherKeyPair instance)
        {
            using (TextWriter textWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(textWriter);

                pemWriter.WriteObject(instance.Private);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
        }

        public static String ToPublicKeyPem(this AsymmetricCipherKeyPair instance)
        {
            using (TextWriter textWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(textWriter);

                pemWriter.WriteObject(instance.Public);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
        }
    }
}
