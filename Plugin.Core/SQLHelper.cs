using Microsoft.Data.SqlClient;

namespace Plugin.Core
{
    internal static class SQLHelper
    {
        public static void ListDatabases(string conString)
        {
            using(var con = new SqlConnection(conString))
            {
                using(var cmd = new SqlCommand($"SELECT name FROM master.sys.databases", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
