using System;
using System.Collections.Generic;
using System.Text;

namespace Division42LLC.WebCA.Models
{
    public class CertificateEntry
    {
        public String DistinguishedName { get; set; }
        public String Thumbprint { get; set; }
        public String SerialNumber { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public String PublicKey { get; set; }

        public DistinguishedNameDetails DistinguishedNameDetails { get; set; }
        public string KeySize { get; internal set; }
    }
}
