using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace Browser_History
{
    public partial class MainWindow : Window
    {
        public GoogleChrome Chrome { get; private set; } = new GoogleChrome();
        public MozillaFirefox Firefox { get; private set; } = new MozillaFirefox();
        public InternetExplorer Explorer { get; private set; } = new InternetExplorer();

        public MainWindow()
        {
            InitializeComponent();

            if(Process.GetProcessesByName("chrome").Length == 0)
                Chrome.GetHistory();
            
            Firefox.GetHistory();
            Explorer.GetHistory();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbBrowsers.SelectedIndex < 0)
            {
                MessageBox.Show("Browser not selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cbBrowsers.SelectedIndex == 0 && Process.GetProcessesByName("chrome").Length != 0)
            {
                MessageBox.Show("Cannot access Chrome history while it's running", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            switch (cbBrowsers.SelectedIndex)
            {
                case 0: Chrome.URLs.Clear(); Chrome.GetHistory(); break;
                case 1: Firefox.URLs.Clear(); Firefox.GetHistory(); break;
                case 2: Explorer.URLs.Clear(); Explorer.GetHistory(); break;
            }
            cbBrowsers_SelectionChanged(sender, null);
            MessageBox.Show("Data updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void cbBrowsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgBrowserData.Items.Clear();

            if (cbBrowsers.SelectedIndex < 0)
                return;

            switch (cbBrowsers.SelectedIndex)
            {
                case 0:
                    foreach (var item in Chrome.URLs)
                    {
                        dgBrowserData.Items.Add(item);
                    }
                    break;
                case 1:
                    foreach (var item in Firefox.URLs)
                    {
                        dgBrowserData.Items.Add(item);
                    }
                    break;
                case 2:
                    foreach (var item in Explorer.URLs)
                    {
                        dgBrowserData.Items.Add(item);
                    }
                    break;
            }
        }
    }
}
