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
using Division42LLC.WebCA.Models;
using System.Text;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class CAController : Controller
    {
        [HttpGet("wipe")]
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

        [HttpGet("download/")]
        public ActionResult Download()
        {
            Console.WriteLine("GET /api/ca/download/");

            String pathAndFilename = CAStorePathInfo.CACertPathAndFileName;

            if (System.IO.File.Exists(pathAndFilename))
            {
                Response.Headers["Content-Disposition"] =
                    new ContentDispositionHeaderValue("attachment")
                    { FileName = $"ca.crt" }.ToString();

                X509Certificate2 result = new X509Certificate2(pathAndFilename, null, X509KeyStorageFlags.Exportable);

                Byte[] fileContents = Encoding.UTF8.GetBytes(result.ExportToPEM());

                return new FileContentResult(fileContents, new MediaTypeHeaderValue("application/octet-stream"));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("download/public/")]
        public ActionResult DownloadPublic()
        {
            Console.WriteLine("GET /api/ca/download/public");

            String pathAndFilename = CAStorePathInfo.CACertPathAndFileName;

                        if (System.IO.File.Exists(pathAndFilename))
            {
                Response.Headers["Content-Disposition"] =
                    new ContentDispositionHeaderValue("attachment")
                    { FileName = $"ca.pub" }.ToString();

                CertificateAndPems result = new CertificateAndPems(new X509Certificate2(pathAndFilename, null, X509KeyStorageFlags.Exportable));

                Byte[] fileContents = Encoding.UTF8.GetBytes(result.PublicKeyPem);

                return new FileContentResult(fileContents, new MediaTypeHeaderValue("application/octet-stream"));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("download/private/")]
        public ActionResult DownloadPrivate()
        {
            Console.WriteLine("GET /api/ca/download/private");

            String pathAndFilename = CAStorePathInfo.CACertPathAndFileName;

            if (System.IO.File.Exists(pathAndFilename))
            {
                Response.Headers["Content-Disposition"] =
                    new ContentDispositionHeaderValue("attachment")
                    { FileName = $"ca.key" }.ToString();

                CertificateAndPems result = new CertificateAndPems(new X509Certificate2(pathAndFilename, null, X509KeyStorageFlags.Exportable));

                Byte[] fileContents = Encoding.UTF8.GetBytes(result.PrivateKeyPem);

                return new FileContentResult(fileContents, new MediaTypeHeaderValue("application/octet-stream"));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("get")]
        public dynamic Get()
        {
            Console.WriteLine("GET /api/ca/get");
            CAManager caManager = new CAManager();

            var cert = caManager.GetCACertificate();
            if (cert == null)
                return new { status = "Certificate not present." };
            else
            {
                return new
                {
                    status = "OK",
                    x509certificate = cert,
                    subjectDN = cert.Certificate.ExtractDNFields(),
                    keySize = $"{cert.Certificate.GetRSAPublicKey().KeySize}-bit",
                    certificatePem = cert.Certificate.ExportToPEM()
                };
            }
        }

        // POST api/values
        [HttpPost("create")]
        public void Create([FromBody]dynamic request)
        {
            Console.WriteLine("POST /api/ca/create");

            CAManager caManager = new CAManager();

            String name = request.fqdn;
            String organization = request.organization;
            String organizationalUnit = request.organizationalUnit;
            String city = request.city;
            String stateCode = request.stateCode;
            String countryCode = request.countryCode;
            String password = null;

            caManager.GenerateNewCACertificate(name, organization, organizationalUnit, city, stateCode, countryCode, password);

        }

    }

}
