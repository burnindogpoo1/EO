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
    public partial class PlantPage : ContentPage
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

        List<PlantInventoryDTO> plants = new List<PlantInventoryDTO>();

        ObservableCollection<PlantInventoryDTO> list2 = new ObservableCollection<PlantInventoryDTO>();

        public PlantPage()
        {
            InitializeComponent();

            List<PlantTypeDTO> plantTypes = GetPlantTypes();

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (PlantTypeDTO code in plantTypes)
            {
                list1.Add(new KeyValuePair<long, string>(code.PlantTypeId, code.PlantTypeName));
            }

            PlantType.ItemsSource = list1;

            PlantType.SelectedIndexChanged += PlantType_SelectedIndexChanged;

            PlantName.SelectedIndexChanged += PlantName_SelectedIndexChanged;

            PlantSize.SelectedIndexChanged += PlantSize_SelectedIndexChanged;

            plants = ((App)App.Current).GetPlants().PlantInventoryList;

            foreach(PlantInventoryDTO p in plants)
            {
                list2.Add(p);
            }

            plantListView.ItemsSource = list2;
        }


        public List<PlantTypeDTO> GetPlantTypes()
        {
            List<PlantTypeDTO> plantTypes = new List<PlantTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse = client.GetAsync("api/Login/GetPlantTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetPlantTypeResponse response = JsonConvert.DeserializeObject<GetPlantTypeResponse>(strData);
                    plantTypes = response.PlantTypes;
                }
                else
                {
                   // MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }
            return plantTypes;
        }

        public GetPlantResponse GetPlantsByType(long plantTypeId)
        {
            GetPlantResponse plants = new GetPlantResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetPlantsByType?plantTypeId=" + plantTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    plants = JsonConvert.DeserializeObject<GetPlantResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {

            }

            return plants;
        }

        private void PlantSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void PlantName_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)PlantType.SelectedItem).Key;

            //List<GetPlantResponse> plants = GetPlantSizes(selectedValue);

            //ObservableCollection<KeyValuePair<long, string>> list3 = new ObservableCollection<KeyValuePair<long, string>>();

            //foreach (GetPlantResponse resp in plants)
            //{
            //    list2.Add(new KeyValuePair<long, string>(resp.Plant.PlantId, resp.Plant.PlantName));
            //}

            //PlantSize.ItemsSource = list3;

            //ObservableCollection<PlantInventoryDTO> pDTO = new ObservableCollection<PlantInventoryDTO>();

            //foreach (PlantInventoryDTO p in plants)
            //{
            //    pDTO.Add(p);
            //}

            //plantListView.ItemsSource = pDTO;
        }

        private void PlantType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)PlantType.SelectedItem).Key;

            GetPlantResponse response = GetPlantsByType(selectedValue);

            plants = response.PlantInventoryList;

            ObservableCollection<KeyValuePair<long, string>> list2 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (PlantInventoryDTO resp in plants)
            {
                list2.Add(new KeyValuePair<long, string>(resp.Plant.PlantId, resp.Plant.PlantName));
            }

            PlantName.ItemsSource = list2;
        }
    }
}