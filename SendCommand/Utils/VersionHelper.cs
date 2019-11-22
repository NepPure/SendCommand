using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SendCommand.Utils
{
    /// <summary>
    /// 版本号帮助
    /// </summary>
    public static class VersionHelper
    {
        public static string GetCurrentVersion()
        {
            var ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            var versions = ver.Split('.');
            return $"{versions[0]}.{versions[1]}.{versions[2]}";
        }
    }
}
