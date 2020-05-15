using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedWorkOrderPage : TabbedPage
    {
        public TabbedWorkOrderPage()
        {
            Children.Add(new WorkOrderPage(this));
            Children.Add(new ImagePage());
            InitializeComponent();
            Initialize();    
        }

        public TabbedWorkOrderPage(long workOrderId)
        {
            Children.Add(new WorkOrderPage(this,workOrderId));
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
        public void AddInventoryImage(EOImgData imageData)
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