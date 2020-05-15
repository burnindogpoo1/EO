using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoliagePage : ContentPage
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

        List<FoliageInventoryDTO> foliage = new List<FoliageInventoryDTO>();

        ObservableCollection<FoliageInventoryDTO> list2 = new ObservableCollection<FoliageInventoryDTO>();

        public FoliagePage()
        {
            InitializeComponent();

            List<FoliageTypeDTO> foliageTypes = GetFoliageTypes();

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (FoliageTypeDTO code in foliageTypes)
            {
                list1.Add(new KeyValuePair<long, string>(code.FoliageTypeId, code.FoliageTypeName));
            }

            FoliageType.ItemsSource = list1;

            FoliageType.SelectedIndexChanged += FoliageType_SelectedIndexChanged;

            FoliageName.SelectedIndexChanged += FoliageName_SelectedIndexChanged;

            FoliageSize.SelectedIndexChanged += FoliageSize_SelectedIndexChanged;

            foreach(FoliageInventoryDTO f in GetFoliage().FoliageInventoryList)
            {
                list2.Add(f);
            }

            foliageListView.ItemsSource = list2;
        }

        public GetFoliageResponse GetFoliage()
        {
            GetFoliageResponse response = new GetFoliageResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(((App)App.Current).LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetFoliage").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<GetFoliageResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public List<FoliageTypeDTO> GetFoliageTypes()
        {
            List<FoliageTypeDTO> foliageTypes = new List<FoliageTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri(((App)App.Current).LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse = client.GetAsync("api/Login/GetFoliageTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetFoliageTypeResponse response = JsonConvert.DeserializeObject<GetFoliageTypeResponse>(strData);
                    foliageTypes = response.FoliageTypes;
                }
                else
                {
                    // MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }
            return foliageTypes;
        }

        public GetFoliageResponse GetFoliageByType(long foliageTypeId)
        {
            GetFoliageResponse foliage = new GetFoliageResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.2:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetFoliageByType?plantTypeId=" + foliageTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    foliage = JsonConvert.DeserializeObject<GetFoliageResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {

            }

            return foliage;
        }

        private void FoliageSize_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void FoliageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //long selectedValue = ((KeyValuePair<long, string>)PlantType.SelectedItem).Key;

            //List<GetPlantResponse> plants = GetPlantSizes(selectedValue);

            //ObservableCollection<KeyValuePair<long, string>> list3 = new ObservableCollection<KeyValuePair<long, string>>();

            //foreach (GetPlantResponse resp in plants)
            //{
            //    list2.Add(new KeyValuePair<long, string>(resp.Plant.PlantId, resp.Plant.PlantName));
            //}

            //PlantSize.ItemsSource = list3; 

            ObservableCollection<FoliageInventoryDTO> fDTO = new ObservableCollection<FoliageInventoryDTO>();

            foreach (FoliageInventoryDTO f in foliage)
            {
                fDTO.Add(f);
            }

            foliageListView.ItemsSource = fDTO;
        }

        private void FoliageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)FoliageType.SelectedItem).Key;

            GetFoliageResponse response = GetFoliageByType(selectedValue);

            foliage = response.FoliageInventoryList;

            ObservableCollection<KeyValuePair<long, string>> list2 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (FoliageInventoryDTO resp in foliage)
            {
                list2.Add(new KeyValuePair<long, string>(resp.Foliage.FoliageId, resp.Foliage.FoliageName));
            }

            FoliageName.ItemsSource = list2;
        }
    }
}