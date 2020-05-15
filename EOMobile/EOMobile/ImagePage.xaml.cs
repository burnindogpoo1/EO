using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        public List<ImageResponse> Images = new List<ImageResponse>();

        public List<EOImageSource> ImageSourceList = new List<EOImageSource>();

        public List<ImageSource> sourceList = new List<ImageSource>();

        public ObservableCollection<ImageSource> ImageSourceListOC = new ObservableCollection<ImageSource>();

        public ImagePage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        public void AddImages(List<ImageResponse> imageResponseList)
        {
            if (imageResponseList != null)
            {
                sourceList.Clear();

                Images = imageResponseList;

                foreach (ImageResponse imgResponse in Images)
                {
                    ImageSource imgSource = ImageSource.FromStream(() => new MemoryStream(imgResponse.Image));

                    sourceList.Add(imgSource);
                }

                ImageSourceListOC = new ObservableCollection<ImageSource>(sourceList);

                imageList.ItemsSource = ImageSourceListOC;
            }
        }

        public void AddToImageList(EOImgData imageData)
        {
            ImageSource imgSource = ImageSource.FromStream(() => new MemoryStream(imageData.imgData));

            sourceList.Add(imgSource);

            ImageSourceListOC = new ObservableCollection<ImageSource>(sourceList);

            imageList.ItemsSource = ImageSourceListOC;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void UpdateImageList(EOImgData imageData)
        {
            try
            {
                ImageSource imgSource = ImageSource.FromStream(() => new MemoryStream(imageData.imgData));

                if(!imageData.isNewImage)
                {

                }

                sourceList.Add(imgSource);

                ImageSourceListOC = new ObservableCollection<ImageSource>(sourceList);

                imageList.ItemsSource = ImageSourceListOC;
            }
            catch(Exception ex)
            {
                int debug = 1;
            }
        }
    }
}