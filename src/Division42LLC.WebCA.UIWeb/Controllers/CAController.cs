using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Division42LLC.WebCA.CA;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class CAController : Controller
    {
        // GET api/values
        [HttpGet]
        public dynamic Get()
        {
            Console.WriteLine("GET /api/CA");
            CAManager caManager = new CAManager();

            var cert = caManager.GetCACertificate();
            if (cert == null)
                return new { status="Certificate not present." };
            else
                return cert;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]String name)
        {
            Console.WriteLine("POST /api/CA");

            CAManager caManager = new CAManager();

            caManager.GenerateNewCACertificate(name);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
