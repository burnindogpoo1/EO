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
	public partial class ContainersPage : ContentPage
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

        List<ContainerInventoryDTO> containers = new List<ContainerInventoryDTO>();

        ObservableCollection<ContainerInventoryDTO> list2 = new ObservableCollection<ContainerInventoryDTO>();

        public ContainersPage ()
		{
			InitializeComponent ();

            List<ContainerTypeDTO> containerTypes = GetContainerTypes();

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (ContainerTypeDTO code in containerTypes)
            {
                list1.Add(new KeyValuePair<long, string>(code.ContainerTypeId, code.ContainerTypeName));
            }

            ContainerType.ItemsSource = list1;

            ContainerType.SelectedIndexChanged += ContainerType_SelectedIndexChanged;

            ContainerName.SelectedIndexChanged += ContainerName_SelectedIndexChanged;

            ContainerSize.SelectedIndexChanged += ContainerSize_SelectedIndexChanged;

            containers = GetContainers().ContainerInventoryList;

            foreach(ContainerInventoryDTO c in containers)
            {
                list2.Add(c);
            }

            containerListView.ItemsSource = list2;
        }

        public GetContainerResponse GetContainers()
        {
            GetContainerResponse response = new GetContainerResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainers").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<GetContainerResponse>(strData);
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

        public List<ContainerTypeDTO> GetContainerTypes()
        {
            List<ContainerTypeDTO> containerTypes = new List<ContainerTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse = client.GetAsync("api/Login/GetContainerTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetContainerTypeResponse response = JsonConvert.DeserializeObject<GetContainerTypeResponse>(strData);
                    containerTypes = response.ContainerTypeList;
                }
                else
                {
                    // MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }
            return containerTypes;
        }

        public GetContainerResponse GetContainerByType(long containerTypeId)
        {
            GetContainerResponse container = new GetContainerResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainerByType?containerTypeId=" + containerTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    container = JsonConvert.DeserializeObject<GetContainerResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {

            }

            return container;
        }

        private void ContainerSize_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void ContainerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //long selectedValue = ((KeyValuePair<long, string>)PlantType.SelectedItem).Key;

            //List<GetPlantResponse> plants = GetPlantSizes(selectedValue);

            //ObservableCollection<KeyValuePair<long, string>> list3 = new ObservableCollection<KeyValuePair<long, string>>();

            //foreach (GetPlantResponse resp in plants)
            //{
            //    list2.Add(new KeyValuePair<long, string>(resp.Plant.PlantId, resp.Plant.PlantName));
            //}

            //PlantSize.ItemsSource = list3; 

            ObservableCollection<ContainerInventoryDTO> cDTO = new ObservableCollection<ContainerInventoryDTO>();

            foreach (ContainerInventoryDTO c in containers)
            {
                cDTO.Add(c);
            }

            containerListView.ItemsSource = cDTO;
        }

        private void ContainerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)ContainerType.SelectedItem).Key;

            GetContainerResponse response = GetContainerByType(selectedValue);

            containers = response.ContainerInventoryList;

            ObservableCollection<KeyValuePair<long, string>> list2 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (ContainerInventoryDTO resp in containers)
            {
                list2.Add(new KeyValuePair<long, string>(resp.Container.ContainerId, resp.Container.ContainerName));
            }

            ContainerName.ItemsSource = list2;
        }
    }
}