using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Division42LLC.WebCA.CA;
using Division42LLC.WebCA.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace app.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CAController : Controller
    {
        [HttpGet]
        public dynamic Wipe()
        {
            Console.WriteLine("GET /api/ca/wipe");
            CAManager caManager = new CAManager();

            try
            {
                Directory.Delete(CAStorePathInfo.CARootPath, true);
                return new { status = "OK" };
            }
            catch (Exception exception)
            {
                return new { status = "FAIL", message = exception.Message };
            }

        }

        [HttpGet]
        public ActionResult Download()
        {
            Console.WriteLine("GET /api/ca/download");

            if (System.IO.File.Exists(CAStorePathInfo.CACertPathAndFileName))
            {
                //Response.Headers.Add("Content-Disposition", "attachment; webCA-cert.pfx");

                Response.Headers["Content-Disposition"] =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "webCA-cert.pfx"
                    }.ToString();

                return new PhysicalFileResult(
                    CAStorePathInfo.CACertPathAndFileName,
                    new MediaTypeHeaderValue("application/octet-stream"));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public dynamic Get()
        {
            Console.WriteLine("GET /api/ca/get");
            CAManager caManager = new CAManager();

            var cert = caManager.GetCACertificate();
            if (cert == null)
                return new { status = "Certificate not present." };
            else
            {
                String publicKey = cert.ExportToPEM();
                return new
                {
                    status = "OK",
                    x509certificate = cert,
                    subjectDN = cert.ExtractDNFields(),
                    publicKey = publicKey
                };
            }
        }

        // POST api/values
        [HttpPost]
        public void Create([FromBody]dynamic request)
        {
            Console.WriteLine("POST /api/ca/create");

            CAManager caManager = new CAManager();

            String name = request.fqdn;
            String organization = request.organization;
            String organizationalUnit = "IT Security";
            String city = request.city;
            String stateCode = request.stateCode;
            String countryCode = request.countryCode;
            String password = null;

            caManager.GenerateNewCACertificate(name, organization, organizationalUnit, city, stateCode, countryCode, password);

        }

    }

}
