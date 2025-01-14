using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_HttpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        readonly HttpClient client = new HttpClient();

        private async void btnFecthData_Click(object sender, RoutedEventArgs e)
        {
            string url = txtURL.Text;
            
            if (!isValidURL(url)) { 
                MessageBox.Show("Please enter correct url!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!await isUrlNotFoundAsync(url))
            {
                MessageBox.Show("The connection is not success!");
                return;
            }

            try
            {
                string responseBody = await client.GetStringAsync(url);
                txtContent.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);    
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtURL.Clear();
            txtContent.Clear();
        }

        private bool isValidURL(string url)
        {
            if (string.IsNullOrEmpty(url) || Regex.IsMatch(url, @"\s"))
            {
                return false;
            }

            return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async Task<bool> isUrlNotFoundAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound) { return false; }
            return true;
        }
    }
}