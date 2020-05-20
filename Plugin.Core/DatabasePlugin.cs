using System;

namespace Plugin.Core
{
    public class DatabasePlugin : MarshalByRefObject
    {
        public void TouchDb(string conString)
        {
            try
            {
                SQLHelper.ListDatabases(conString);
                Console.WriteLine($"Listed databases using AppDomain {AppDomain.CurrentDomain.FriendlyName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException?.Message + Environment.NewLine + ex.InnerException?.StackTrace);
            }
        }
    }
}
