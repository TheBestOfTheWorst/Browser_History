using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace Browser_History
{
    public class GoogleChrome
    {
        public List<URL> URLs { get; private set; }  = new List<URL>();

        public IEnumerable<URL> GetHistory()
        {
            // Get Current Users App Data
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] tempstr = documentsFolder.Split('\\');
            string tempstr1 = "";

            documentsFolder += "\\Google\\Chrome\\User Data\\Default";
            if (tempstr[tempstr.Length - 1] != "Local")
            {
                for (int i = 0; i < tempstr.Length - 1; i++)
                {
                    tempstr1 += tempstr[i] + "\\";
                }
                documentsFolder = tempstr1 + "Local\\Google\\Chrome\\User Data\\Default";
            }

            // Check if directory exists
            return Directory.Exists(documentsFolder) ? ExtractUserHistory(documentsFolder) : null;
        }
        private IEnumerable<URL> ExtractUserHistory(string folder)
        {
            // Get User history info
            DataTable historyDT = ExtractFromTable("urls", folder);

            // Loop each history entry
            foreach (DataRow row in historyDT.Rows)
            {

                // Obtain URL, Title and Visit Time strings
                string url = row["url"].ToString();
                string title = row["title"].ToString();
                string visitTime = row["last_visited"].ToString();

                // Create new Entry
                URL u = new URL(url.Replace('\'', ' '), title.Replace('\'', ' '), visitTime);

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
            string dbPath = folder + "\\History";

            // If file exists
            if (File.Exists(dbPath))
            {
                // Data connection
                sql_con = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;New=False;Compress=True;");

                // Open the Connection
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();

                // Select Query
                string CommandText = "SELECT datetime(last_visit_time/ 1000000 - 11644473600, \"unixepoch\") AS last_visited, url, title FROM " + table;

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
