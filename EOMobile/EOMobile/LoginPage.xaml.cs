using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EOMobile
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //File.WriteAllText(_fileName, editor.Text);
            Navigation.PushAsync(new NavigationPage(new MainPage()));
        }
    }
}
