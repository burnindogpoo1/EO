using EOMobile.Interfaces;
using EOMobile.XamarinFormsCamera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();

            StartCamera();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public async void SelectImageClicked(object sender, EventArgs args)
        {
            //StartCamera replaced this
        }

        public async void StartCamera()
        {
            var action = await DisplayActionSheet("Add Photo", "Cancel", null, "Choose Existing", "Take Photo");

            if (action == "Choose Existing")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<ICameraInterface>().LaunchGallery(FileFormatEnum.JPEG, fileName);
                });
            }
            else if (action == "Take Photo")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<ICameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                });
            }
        }

        /*
         *  Setting the file name is really only needed for Android, when in the OnActivityResult method you need
         *  a way to know the file name passed into the intent when launching the camera/gallery. In this case,
         *  1 image will be saved to the file system using the value of App.DefaultImageId, this is required for the 
         *  FileProvider implemenation that is needed on newer Android OS versions. Using the same file name will 
         *  keep overwriting the existing image so you will not fill up the app's memory size over time. 
         * 
         *  This of course assumes your app has NO need to save images locally. But if your app DOES need to save images 
         *  locally, then pass the file name you want to use into the method SetImageFileName (do NOT include the file extension in the name,
         *  that will be handled down the road based on the FileFormatEnum you pick). 
         * 
         *  NOTE: When saving images, if you decide to pick PNG format, you may notice your app runs slower 
         *  when processing the image. If your image doesn't need to respect any Alpha values, use JPEG, it's faster. 
         */

        private string SetImageFileName(string fileName = null)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (fileName != null)
                    App.ImageIdToSave = fileName;
                else
                    App.ImageIdToSave = CameraApp.DefaultImageId;

                return App.ImageIdToSave;
            }
            else
            {
                //To iterate, on iOS, if you want to save images to the devie, set 
                if (fileName != null)
                {
                    App.ImageIdToSave = fileName;
                    return fileName;
                }
                else
                    return null;
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}