using KarmaRewards.Infrastructure.ErrorSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace KarmaRewards.Infrastructure.Hosting
{
    public static class Paths
    {
        private static string _appPath = string.Empty;
        public static string GetAppPath()
        {

            if (string.IsNullOrWhiteSpace(_appPath) == true)
            {
                string path = string.Empty;
                if (TryGetAspNetPath(out path) == true)
                    _appPath = path;
                else if (TryGetWcfPath(out path) == true)
                    _appPath = path;
                else if (TryGetWindowsAppPath(out path) == true)
                    _appPath = path;
                else
                    throw new HostRuntimeException("Unable to resolve app deployment path.");
            }
            return _appPath;
        }

        private static bool TryGetWindowsAppPath(out string path)
        {
            path = string.Empty;
            try
            {
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrWhiteSpace(path) == true)
                    return false;
                if (path.EndsWith(@"\") == true)
                    path = path.TrimEnd('\\');
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryGetWcfPath(out string path)
        {
            path = string.Empty;
            try
            {
                var currentContext = OperationContext.Current;
                if (currentContext == null)
                    return false;
                else
                    path = HostingEnvironment.ApplicationPhysicalPath;
                if (string.IsNullOrWhiteSpace(path) == true)
                    return false;
                if (path.EndsWith(@"\") == true)
                    path = path.TrimEnd('\\');
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryGetAspNetPath(out string path)
        {
            path = string.Empty;
            try
            {
                if (HttpRuntime.AppDomainId == null)
                    return false;
                path = HttpRuntime.BinDirectory;
                if (string.IsNullOrWhiteSpace(path) == true)
                    return false;
                // if path is of type ..\bin\
                if (path.EndsWith(@"\") == true)
                    path = Path.GetDirectoryName(Path.GetDirectoryName(path));
                // if path is of type ..\bin
                else
                    path = Path.GetDirectoryName(path);
                return true;
            }
            catch
            {
                path = string.Empty;
                return false;
            }
        }

        public static string MapFile(string fileName)
        {
            return GetAppPath() + @"\" + fileName;
        }
    }
}
