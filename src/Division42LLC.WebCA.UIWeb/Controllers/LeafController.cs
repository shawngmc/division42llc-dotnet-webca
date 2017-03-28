﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Division42LLC.WebCA.CA;
using Division42LLC.WebCA.Extensions;
using Division42LLC.WebCA.Models;
using System.IO;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Division42LLC.WebCA.UIWeb.Controllers
{
    [Route("api/[controller]")]
    public class LeafController : Controller
    {
        [HttpGet("get/{thumbprint}")]
        public dynamic Get(String thumbprint)
        {
            Console.WriteLine($"GET /api/leaf/get/{thumbprint}");
            CAManager caManager = new CAManager();

            var cert = caManager.GetLeafCertificate(thumbprint);
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


        [HttpGet("download/{thumbprint}")]
        public ActionResult Download(String thumbprint)
        {
            Console.WriteLine("GET /api/leaf/download/" + thumbprint);

            String pathAndFilename = Path.Combine(CAStorePathInfo.LeafCertPath, $"{thumbprint}.pfx");

            if (System.IO.File.Exists(pathAndFilename))
            {
                Response.Headers["Content-Disposition"] =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{thumbprint}.pfx"
                    }.ToString();

                return new PhysicalFileResult(
                    pathAndFilename,
                    new MediaTypeHeaderValue("application/octet-stream"));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("delete/{thumbprint}")]
        public ActionResult Delete(String thumbprint)
        {
            Console.WriteLine("GET /api/leaf/delete/" + thumbprint);

            String pathAndFilename = Path.Combine(CAStorePathInfo.LeafCertPath, $"{thumbprint}.pfx");

            if (System.IO.File.Exists(pathAndFilename))
            {
                System.IO.File.Delete(pathAndFilename);

                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("")]
        public dynamic GetAll()
        {
            Console.WriteLine("GET /api/leaf/getall");
            CAManager caManager = new CAManager();

            IEnumerable<CertificateEntry> certs = caManager.GetAllLeafCertificates();
            if (certs == null || certs.Count() == 0)
                return new { status = "Certificates not present." };
            else
            {
                return certs;
            }
        }

        // POST api/values
        [HttpPost("create")]
        public void Create([FromBody]dynamic request)
        {
            Console.WriteLine("POST /api/leaf/create");

            CAManager caManager = new CAManager();

            String name = request.fqdn;
            String organization = request.organization;
            String organizationalUnit = "IT Security";
            String city = request.city;
            String stateCode = request.stateCode;
            String countryCode = request.countryCode;
            String password = null;

            caManager.GenerateNewLeafCertificate(name, organization, organizationalUnit, city, stateCode, countryCode, password);

            //return Ok();

        }
    }
}
