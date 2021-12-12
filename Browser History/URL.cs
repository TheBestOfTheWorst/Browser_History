namespace Browser_History
{
    public class URL
    {
        public string Url { get; set; } = "";
        public string Title { get; set; } = "";
        public string LastVisit { get; set; } = "";
        public URL(string url, string title, string lastVisit)
        {
            Url = url;
            Title = title;
            LastVisit = lastVisit;
        }
        public override string ToString() => Title + " - " + Url + " - " + LastVisit;
    }
}
