using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedSiteServicePage : TabbedPage
    {
        public TabbedSiteServicePage()
        {
            Children.Add(new SiteServicePage(this));
            Children.Add(new ImagePage());
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((App)App.Current).ClearImageData();
            ((App)App.Current).ClearImageDataList();
        }
    }
}