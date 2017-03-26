using System;
using System.Collections.Generic;
using System.Text;

namespace Division42LLC.WebCA.CA
{
    public static class CAStorePathInfo
    {
        public static String CARootPath { get; set; } = "/var/localCA";

        public static String CACertPath { get; set; } = $"{CARootPath}/CA";
        public static String CACertPathAndFileName { get; set; } = $"{CACertPath}/ca-cert.sst";

        public static String LeafCertPath { get; set; } = $"{CARootPath}/leaf";
    }
}
