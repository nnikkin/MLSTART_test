using MallenomTest.Models;
using MallenomTest.Services;
using MallenomTest.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;

namespace MallenomTest
{
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            DialogService dialogService = new DialogService();
            DataContext = new MainViewModel(dialogService);

            client.BaseAddress = new Uri("https://localhost:7278/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            InitializeComponent();
            this.GetImageFiles();
        }

        private async void GetImageFiles()
        {
            try
            {
                var response = await client.GetStringAsync("api/images/all");
                var images = JsonConvert.DeserializeObject<List<Image>>(response);
                ImagesDataGrid.DataContext = images;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or show a message to the user
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

    }
}