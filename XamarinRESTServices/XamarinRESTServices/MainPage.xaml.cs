using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace XamarinRESTServices
{
    public partial class MainPage : ContentPage
    {
        private const string url = "https://jsonplaceholder.typicode.com/posts";
        private HttpClient _Client = new HttpClient();
        private ObservableCollection<Post> _post;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var content = await _Client.GetStringAsync(url);
            var post = JsonConvert.DeserializeObject<List<Post>>(content);
            _post = new ObservableCollection<Post>(post);
            Post_List.ItemsSource = _post;
            base.OnAppearing();
        }

        private async void OnAdd_Click(object sender, EventArgs e)
        {
            var post = new Post() { Title = "Title" + DateTime.Now.Ticks, Body = "Body" };
            _post.Insert(0, post);
            var content = JsonConvert.SerializeObject(post);
            await _Client.PostAsync(url, new StringContent(content));
        }

        private async void OnDelete_Click(object sender, EventArgs e)
        {
            var post = _post[0];
            _post.Remove(post);
            await _Client.DeleteAsync(url + "/" + post.Id);
        }
    }
}
