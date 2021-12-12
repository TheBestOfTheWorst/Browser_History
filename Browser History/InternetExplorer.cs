using System.Collections.Generic;
using UrlHistoryLibrary;

namespace Browser_History
{
    public class InternetExplorer
    {
        // List of URL objects
        public List<URL> URLs { get; private set; } = new List<URL>();

        public IEnumerable<URL> GetHistory()
        {
            // Initiate main object
            UrlHistoryWrapperClass urlhistory = new UrlHistoryWrapperClass();

            // Enumerate URLs in History
            UrlHistoryWrapperClass.STATURLEnumerator enumerator = urlhistory.GetEnumerator();

            // Iterate through the enumeration
            while (enumerator.MoveNext())
            {
                //if (string.IsNullOrEmpty(enumerator.Current.Title)) continue;

                // Obtain URL and Title
                string url = enumerator.Current.URL?.Replace('\'', ' ');

                // In the title, eliminate single quotes to avoid confusion
                string title = enumerator.Current.Title?.Replace('\'', ' ');

                string visitedDate = enumerator.Current.LastVisited.ToString();

                // Create new entry
                URL u = new URL(url, title, visitedDate);

                // Add entry to list
                URLs.Add(u);
            }

            return URLs;
        }
    }
}
