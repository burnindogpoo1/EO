using EO.ViewModels.ControllerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EOMobile
{
    public partial class LoginPage : ContentPage
    {
        public string User 
        {
            get { return ((App)App.Current).User; }
            set { ((App)App.Current).User = value; } 
        }

        public string Pwd
        {
            get { return ((App)App.Current).Pwd; }
            set { ((App)App.Current).Pwd = value; }
        }

        public LoginPage()
        {
            InitializeComponent();
        }

        void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                
                User = this.Name.Text;
                Pwd = this.Password.Text;

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                LoginRequest request = new LoginRequest(User, Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/Login", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    IEnumerable<string> values;
                    httpResponse.Headers.TryGetValues("EO-Header", out values);
                    if (values != null && values.ToList().Count == 1)
                    {
                        // this.MainContent.Content = new Frame() { Content = new DashboardPage() };
                        Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                       // MessageBox.Show("Unrecognized username / password");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
