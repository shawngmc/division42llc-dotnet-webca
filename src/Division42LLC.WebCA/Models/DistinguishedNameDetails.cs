using System;
using System.Collections.Generic;
using System.Text;

namespace Division42LLC.WebCA.Models
{
    public class DistinguishedNameDetails
    {
        /// <summary>
        /// Gets or sets the country code, or "C" element of the distinguished name.
        /// </summary>
        public String CountryCode { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the organization, or "O" element of the distinguished name.
        /// </summary>
        public String OrganizationName { get; set; }
        
        /// <summary>
        /// Gets or sets the area or department within the organization, or "OU" element of the distinguished name.
        /// </summary>
        public String OrganizationalUnit { get; set; }

        /// <summary>
        /// Gets or sets the city or location, or "L" element of the distinguished name.
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// Gets or sets the province or state code, or "ST" element of the distinguished name.
        /// </summary>
        public String StateCode { get; set; }

        /// <summary>
        /// Gets or sets the name or common name (the subject), or "CN" element of the distinguished name.
        /// </summary>
        public String CommonName { get; set; }
    }
}
