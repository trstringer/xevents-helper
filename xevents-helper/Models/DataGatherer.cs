using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace xevents_helper.Models
{
    public class DataGatherer
    {
        private string _connectionString;

        public DataGatherer()
        {
#if DEBUG
            _connectionString = ConfigurationManager.ConnectionStrings["local_db"].ConnectionString;
#else
            _connectionString = ConfigurationManager.ConnectionStrings["cloud_db"].ConnectionString;
#endif
        }

        public IEnumerable<Release> GetAllReleases()
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.ReleaseGet";

                sda.Fill(output);

                foreach (DataRow row in output.Rows)
                    yield return new Release() { Name = row["FriendlyName"].ToString() };
            }
        }
        public Release GetRelease(string releaseName)
        {
            IEnumerable<Release> allReleases = GetAllReleases();
            if (allReleases == null)
                return null;

            return allReleases.Where(m => m.Name == releaseName).First();
        }

        public IEnumerable<XeEvent> GetAllEventsForRelease(Release release)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.EventSearchByName";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                    {
                        Value = release.Name
                    });

                sda.Fill(output);
            }

            foreach (DataRow row in output.Rows)
                yield return new XeEvent() 
                { 
                    Name = row["name"].ToString(), 
                    Description = row["description"].ToString() 
                };
        }

        private IEnumerable<XeEvent> SearchEventsByName(Release release, string searchString)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.EventSearchByName";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                    {
                        Value = release.Name
                    });
                sqlCmd.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.VarChar, 512)
                    {
                        Value = searchString
                    });

                sda.Fill(output);
            }

            foreach (DataRow row in output.Rows)
                yield return new XeEvent()
                {
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString()
                };
        }
        private IEnumerable<XeEvent> SearchEventsByDescription(Release release, string searchString)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.EventSearchByDescription";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                {
                    Value = release.Name
                });
                sqlCmd.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.VarChar, 512)
                {
                    Value = searchString
                });

                sda.Fill(output);
            }

            foreach (DataRow row in output.Rows)
                yield return new XeEvent()
                {
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString()
                };
        }
        public IEnumerable<XeEvent> SearchEvents(Release release, string searchString, SearchOption searchOption)
        {
            List<XeEvent> events = new List<XeEvent>();

            if (searchOption == SearchOption.ByName || searchOption == SearchOption.ByNameAndDescription)
                events.AddRange(SearchEventsByName(release, searchString));
            if (searchOption == SearchOption.ByDescription || searchOption == SearchOption.ByNameAndDescription)
                events.AddRange(SearchEventsByDescription(release, searchString));

            return events;
        }

        public string GetEventDescription(string releaseName, string eventName)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.EventGetDescription";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                    {
                        Value = releaseName
                    });
                sqlCmd.Parameters.Add(new SqlParameter("@EventName", SqlDbType.NVarChar, 128)
                    {
                        Value = eventName
                    });

                sda.Fill(output);
            }

            if (output.Rows.Count >= 1)
                return output.Rows[0]["description"].ToString();
            else
                return string.Empty;
        }
    }
}