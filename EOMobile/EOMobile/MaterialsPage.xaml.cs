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
	public partial class MaterialsPage : ContentPage
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

        List<MaterialInventoryDTO> materials = new List<MaterialInventoryDTO>();

        ObservableCollection<MaterialInventoryDTO> list2 = new ObservableCollection<MaterialInventoryDTO>();

        public MaterialsPage ()
		{
			InitializeComponent ();

            List<MaterialTypeDTO> materialTypes = ((App)App.Current).GetMaterialTypes();

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (MaterialTypeDTO code in materialTypes)
            {
                list1.Add(new KeyValuePair<long, string>(code.MaterialTypeId, code.MaterialTypeName));
            }

            MaterialType.ItemsSource = list1;

            MaterialType.SelectedIndexChanged += MaterialType_SelectedIndexChanged;

            MaterialName.SelectedIndexChanged += MaterialName_SelectedIndexChanged;

            MaterialSize.SelectedIndexChanged += MaterialSize_SelectedIndexChanged;

            materials = GetMaterials().MaterialInventoryList;

            foreach(MaterialInventoryDTO m in materials)
            {
                list2.Add(m);
            }

            materialListView.ItemsSource = list2;
        }

        public GetMaterialResponse GetMaterials()
        {
            GetMaterialResponse response = new GetMaterialResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetMaterials").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<GetMaterialResponse>(strData);
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
               
        public GetMaterialResponse GetMaterialByType(long materialTypeId)
        {
            GetMaterialResponse material = new GetMaterialResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetMaterialByType?materialTypeId=" + materialTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    material = JsonConvert.DeserializeObject<GetMaterialResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {

            }

            return material;
        }

        private void MaterialSize_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void MaterialName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //long selectedValue = ((KeyValuePair<long, string>)PlantType.SelectedItem).Key;

            //List<GetPlantResponse> plants = GetPlantSizes(selectedValue);

            //ObservableCollection<KeyValuePair<long, string>> list3 = new ObservableCollection<KeyValuePair<long, string>>();

            //foreach (GetPlantResponse resp in plants)
            //{
            //    list2.Add(new KeyValuePair<long, string>(resp.Plant.PlantId, resp.Plant.PlantName));
            //}

            //PlantSize.ItemsSource = list3; 

            ObservableCollection<MaterialInventoryDTO> mDTO = new ObservableCollection<MaterialInventoryDTO>();

            foreach (MaterialInventoryDTO m in materials)
            {
                mDTO.Add(m);
            }

            materialListView.ItemsSource = mDTO;
        }

        private void MaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)MaterialType.SelectedItem).Key;

            GetMaterialResponse response = GetMaterialByType(selectedValue);

            materials = response.MaterialInventoryList;

            ObservableCollection<KeyValuePair<long, string>> list2 = new ObservableCollection<KeyValuePair<long, string>>();

            foreach (MaterialInventoryDTO resp in materials)
            {
                list2.Add(new KeyValuePair<long, string>(resp.Material.MaterialId, resp.Material.MaterialName));
            }

            MaterialName.ItemsSource = list2;
        }
    }
}