using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class CAController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            String args = "x509 -in $CA_ROOT/newcerts/ca-server.cert -subject -issuer -dates -text | awk '/-----BEGIN CERTIFICATE-----/ { show=1 } show; /-----END CERTIFICATE-----/ { show=0 }' | sed \"s:\n:|:g\"";

            using (MemoryStream stream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(stream))
            using (StreamReader reader = new StreamReader(stream))
            {
                ShellExecutioner.ShellExecute("/usr/bin", "openssl", args, writer, null, null);

                stream.Flush();
                stream.Position = 0;

                String commandOutput = reader.ReadToEnd();



                return new string[] { commandOutput };
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
