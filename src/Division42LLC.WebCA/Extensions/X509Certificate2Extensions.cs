using Division42LLC.WebCA.Models;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Division42LLC.WebCA.Extensions
{
    public static class X509Certificate2Extensions
    {
        //public static String ToCertificatePem(this X509Certificate instance)
        //{
        //    using (TextWriter textWriter = new StringWriter())
        //    {
        //        PemWriter pemWriter = new PemWriter(textWriter);

        //        pemWriter.WriteObject(instance);
        //        pemWriter.Writer.Flush();

        //        return textWriter.ToString();
        //    }
        //}

        /// <summary>
        /// Export a certificate to a PEM format string
        /// </summary>
        /// <param name="cert">The certificate to export</param>
        /// <returns>A PEM encoded string</returns>
        public static string ExportToPEM(this X509Certificate cert)
        {
            String certificateRaw = Convert.ToBase64String(cert.Export(X509ContentType.Cert));

            IEnumerable<String> certificateLines = SeparateIntoLines(certificateRaw, 64);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");

            foreach (String certificateLine in certificateLines)
            {
                builder.AppendLine(certificateLine);
            }

            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public static DistinguishedNameDetails ExtractDNFields(this X509Certificate cert)
        {
            DistinguishedNameDetails model = new DistinguishedNameDetails();

            String[] subjectParts = cert.Subject.Split(',');
            foreach (String subjectPart in subjectParts)
            {
                String[] elementParts = subjectPart.Split('=');
                String key = elementParts[0].Trim();
                String value = elementParts[1].Trim();

                if (key == "CN")
                    model.CommonName = value;
                if (key == "O")
                    model.OrganizationName = value;
                if (key == "OU")
                    model.OrganizationalUnit = value;
                if (key == "L")
                    model.City = value;
                if (key == "ST")
                    model.StateCode = value;
                if (key == "C")
                    model.CountryCode = value;
            }

            return model;
        }

        private static IEnumerable<String> SeparateIntoLines(String line, Int32 length)
        {
            String input = line;
            while (input.Length > length)
            {
                String result = input.Substring(0, length);
                input = input.Substring(length);
                yield return result;
            }
            yield return input;
        }
    }
}
