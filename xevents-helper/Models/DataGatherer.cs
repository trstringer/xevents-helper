﻿using System;
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

        public IEnumerable<Event> GetAllEventsForRelease(Release release)
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
                yield return new Event() 
                { 
                    Name = row["name"].ToString(), 
                    Description = row["description"].ToString() 
                };
        }

        private IEnumerable<Event> SearchEventsByName(Release release, string searchString)
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
                yield return new Event()
                {
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString()
                };
        }
        private IEnumerable<Event> SearchEventsByDescription(Release release, string searchString)
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
                yield return new Event()
                {
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString()
                };
        }
        public IEnumerable<Event> SearchEvents(Release release, string searchString, SearchOption searchOption)
        {
            List<Event> events = new List<Event>();

            if (searchOption == SearchOption.ByName || searchOption == SearchOption.ByNameAndDescription)
                events.AddRange(SearchEventsByName(release, searchString));
            if (searchOption == SearchOption.ByDescription || searchOption == SearchOption.ByNameAndDescription)
                events.AddRange(SearchEventsByDescription(release, searchString));

            return events;
        }

        public string GetEventDescription(Release release, string eventName)
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
                        Value = release.Name
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

        public IEnumerable<Action> GetAllActions(Release release)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.ActionGetByRelease";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                    {
                        Value = release.Name
                    });

                sda.Fill(output);
            }

            foreach (DataRow row in output.Rows)
                yield return new Action()
                {
                    Name = row["name"].ToString()
                };
        }

        public IEnumerable<EventField> GetAllEventFieldsForEvent(Release release, string eventName)
        {
            DataTable output = new DataTable();

            using (SqlConnection databaseConnection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
            {
                sqlCmd.Connection = databaseConnection;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.EventColumnSearchByEvent";

                sqlCmd.Parameters.Add(new SqlParameter("@ReleaseName", SqlDbType.VarChar, 32)
                    {
                        Value = release.Name
                    });
                sqlCmd.Parameters.Add(new SqlParameter("@EventName", SqlDbType.NVarChar, 128)
                    {
                        Value = eventName
                    });

                sda.Fill(output);
            }

            foreach (DataRow row in output.Rows)
                yield return new EventField()
                {
                    Name = row["name"].ToString(),
                    TypeName = row["type_name"].ToString(),
                    IsOptional = Convert.ToBoolean(row["is_optional"]),
                    Description = row["description"].ToString()
                };
        }
    }
}