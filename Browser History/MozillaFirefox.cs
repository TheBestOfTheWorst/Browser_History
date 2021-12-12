using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Browser_History
{
    public class MozillaFirefox
    {
        public List<URL> URLs { get; private set; } = new List<URL>();

        public IEnumerable<URL> GetHistory()
        {
            // Get Current Users App Data
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Move to Firefox Data
            documentsFolder += "\\Mozilla\\Firefox\\Profiles\\";

            // Check if directory exists
            if (Directory.Exists(documentsFolder))
            {
                // Loop each Firefox Profile
                foreach (string folder in Directory.GetDirectories(documentsFolder))
                {
                    // Fetch Profile History
                    return ExtractUserHistory(folder);
                }
            }
            return null;
        }
        private IEnumerable<URL> ExtractUserHistory(string folder)
        {
            // Get User history info
            DataTable historyDT = ExtractFromTable("moz_places", folder);

            // Loop each history entry
            foreach (DataRow row in historyDT.Rows)
            {
                // Obtain URL and Title strings
                string url = row["url"].ToString();
                string title = row["title"].ToString();
                string visited = row["last_visited"].ToString();

                // Create new Entry
                URL u = new URL(url.Replace('\'', ' '), title.Replace('\'', ' '), visited);

                // Add entry to list
                URLs.Add(u);
            }

            return URLs;
        }
        private DataTable ExtractFromTable(string table, string folder)
        {
            SQLiteConnection sql_con;
            SQLiteCommand sql_cmd;
            SQLiteDataAdapter DB;
            DataTable DT = new DataTable();

            // FireFox database file
            string dbPath = folder + "\\places.sqlite";

            // If file exists
            if (File.Exists(dbPath))
            {
                // Data connection
                sql_con = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;New=False;Compress=True;");

                // Open the Connection
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();

                // Select Query
                string CommandText = "SELECT datetime(last_visit_date/ 1000000, \"unixepoch\") AS last_visited, url, title FROM " + table;

                // Populate Data Table
                DB = new SQLiteDataAdapter(CommandText, sql_con);
                DB.Fill(DT);

                // Clean up
                sql_con.Close();
            }
            return DT;
        }
    }
}
