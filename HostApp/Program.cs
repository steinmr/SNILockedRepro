using Plugin.Core;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace HostApp
{
    class Program
    {
        static void Main()
        {
            var conString = ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString;

            // Load plugin into new appdomain, do something and then unload it.
            Console.WriteLine("Loading plugin and querying database.");
            var appDomain = CreateAppDomain(new FileInfo(typeof(DatabasePlugin).Assembly.Location));
            var proxy = CreateInstanceFromAndUnwrap<DatabasePlugin>(appDomain);
            proxy.TouchDb(conString); // This will load and lock SNI.dll
            AppDomain.Unload(appDomain);
            Console.WriteLine("Done. Unloading appdomain.");

            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Console.WriteLine();
            Console.WriteLine("Press Enter to start deleting files:");
            Console.ReadKey();
            // This would work fine since the file is shadow copied (but will not do it, as it prevents the app from starting later)
            //File.Delete(Path.Combine(dir, "Microsoft.Data.SqlClient.dll"));

            // One of these will fail
            File.Delete(Path.Combine(dir, "x86", "SNI.dll"));
            File.Delete(Path.Combine(dir, "x64", "SNI.dll"));

            Console.WriteLine("Press the 'æ' key to exit.");
            Console.ReadKey();
        }

        private static AppDomain CreateAppDomain(FileInfo file)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ShadowCopyFiles = "true"
            };

            return AppDomain.CreateDomain(file.Name, null, appDomainSetup);
        }

        protected static T CreateInstanceFromAndUnwrap<T>(AppDomain appDomain) where T : MarshalByRefObject
        {
            var type = typeof(T);
            return (T)appDomain.CreateInstanceFromAndUnwrap(type.Assembly.Location, type.FullName);
        }
    }
}
