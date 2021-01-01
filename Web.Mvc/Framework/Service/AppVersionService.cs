using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Web.Mvc.Framework.Service
{
    public class AppVersionService : IAppVersionService
    {
        public string Version => Assembly
           .GetEntryAssembly()
           .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
           .InformationalVersion;
    }
}
