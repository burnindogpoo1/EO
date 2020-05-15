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
    public partial class SiteServiceReportPage : ContentPage
    {
        TabbedSiteServiceReportPage TabParent;

        public SiteServiceReportPage(TabbedSiteServiceReportPage tabParent)
        {
            TabParent = tabParent;
            InitializeComponent();
        }

        private void ShowSiteServiceReports_Clicked(object sender, EventArgs e)
        {

        }

        private void ShowInventory_Clicked(object sender, EventArgs e)
        {

        }

        private void EditInventory_Clicked(object sender, EventArgs e)
        {

        }
    }
}