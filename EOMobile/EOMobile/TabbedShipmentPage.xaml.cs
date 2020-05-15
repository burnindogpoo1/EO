using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedShipmentPage : TabbedPage
    {
        public TabbedShipmentPage()
        {
            Children.Add(new ShipmentPage(this));
            Children.Add(new ImagePage());
            InitializeComponent();
            Initialize();
        }

        public TabbedShipmentPage(long shipmentId)
        {
            Children.Add(new ShipmentPage(this,shipmentId));
            Children.Add(new ImagePage());
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            MessagingCenter.Subscribe<EOImgData>(this, "ImageSaved", async (arg) =>
            {
                ((ImagePage)Children[1]).UpdateImageList(arg);
            });
        }

        public void LoadShipmentImages(List<ImageResponse> imageResponseList)
        {
            ((ImagePage)Children[1]).AddImages(imageResponseList);
        }

        public void AddShipmentImage(EOImgData imageData)
        {
            ((ImagePage)Children[1]).AddToImageList(imageData);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((App)App.Current).ClearImageData();
            ((App)App.Current).ClearImageDataList();
        }
    }
}