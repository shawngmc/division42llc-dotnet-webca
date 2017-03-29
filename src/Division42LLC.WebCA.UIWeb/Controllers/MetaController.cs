using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Division42LLC.WebCA.UIWeb.Controllers
{
    [Route("api/[controller]")]
    public class MetaController : Controller
    {
        [HttpGet]
        public dynamic Get()
        {
            //$rootScope.app_name = data.app_name;
            //$rootScope.app_version = data.app_version;
            //$rootScope.app_built = data.app_built;
            //$rootScope.hostname = data.hostname;
            //$rootScope.uptime = data.uptime;

            String applicationName = !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("IMAGE_NAME")) ? Environment.GetEnvironmentVariable("IMAGE_NAME") : "[NotInDocker]";
            String applicationVersion = !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("IMAGE_VERSION")) ? Environment.GetEnvironmentVariable("IMAGE_VERSION") : "[NotInDocker]";

            //String applicationName = Environment.GetEnvironmentVariable("IMAGE_NAME");
            //String applicationVersion = Environment.GetEnvironmentVariable("IMAGE_NAME");


            DateTime applicationBuilt = new FileInfo(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase, UriKind.Absolute).LocalPath).LastWriteTime;
            String hostname = Environment.MachineName;

            return new
            {
                ApplicationName = applicationName,
                ApplicationVersion = applicationVersion,
                ApplicationBuilt = applicationBuilt,
                Hostname = hostname
            };
        }
    }
}
