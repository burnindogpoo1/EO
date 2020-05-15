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
	public partial class InventoryPage : ContentPage
	{
		public InventoryPage ()
		{
			InitializeComponent ();
        }

        public void OnPlantsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PlantPage());
        }

        public void OnFoliageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoliagePage());
        }

        public void OnMaterialsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MaterialsPage());
        }

        public void OnContainersClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContainersPage());
        }

        public void OnArrangementsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedArrangementPage());
        }

        //public void OnImportClicked(object sender, EventArgs e)
        //{
        //    //Navigation.PushAsync(new PlantPage());
        //}
    }
}