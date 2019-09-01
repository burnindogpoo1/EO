using EO.ViewModels.ControllerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EOMobile
{
    public partial class App : Application
    {
        public string User { get; set; }

        public string Pwd { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        public List<ArrangementInventoryDTO> GetArrangements()
        {
            List<ArrangementInventoryDTO> arrangements = new List<ArrangementInventoryDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetArrangements").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    GetArrangementResponse response = JsonConvert.DeserializeObject<GetArrangementResponse>(strData);

                    arrangements = response.ArrangementList;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving arrangements");
                }
            }
            catch(Exception ex)
            {

            }

            return arrangements;
        }

        public List<InventoryTypeDTO> GetInventoryTypes()
        {
            List<InventoryTypeDTO> dtoList = new List<InventoryTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));


                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                   client.GetAsync("api/Login/GetInventoryTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetInventoryTypeResponse response = JsonConvert.DeserializeObject<GetInventoryTypeResponse>(strData);
                    dtoList = response.InventoryType;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving inventory types");
                }
            }
            catch (Exception ex)
            {

            }
            return dtoList;
        }

        public List<PersonAndAddressDTO> GetCustomers(GetPersonRequest request)
        {
            List<PersonAndAddressDTO> people = new List<PersonAndAddressDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.PostAsync("api/Login/GetPerson",content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetPersonResponse resp = JsonConvert.DeserializeObject<GetPersonResponse>(strData);
                    people= resp.PersonAndAddress;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving vendors");
                }
            }
            catch(Exception ex)
            {

            }

            return people;
        }

        public List<VendorDTO> GetVendors(GetPersonRequest request)
        {
            List<VendorDTO> vDTO = new List<VendorDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.PostAsync("api/Login/GetVendors",content).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetVendorResponse resp = JsonConvert.DeserializeObject<GetVendorResponse>(strData);
                    vDTO = resp.VendorList;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving vendors");
                }
            }
            catch (Exception ex)
            {

            }

            return vDTO;
        }

        public List<WorkOrderInventoryDTO> GetWorkOrders(WorkOrderListFilter filter)
        {
            List<WorkOrderInventoryDTO> workOrders = new List<WorkOrderInventoryDTO>();

            try
            {
                //WorkOrderListFilter filter = new WorkOrderListFilter();
                //filter.FromDate = this.FromDatePicker.SelectedDate.Value;
                //filter.ToDate = this.ToDatePicker.SelectedDate.Value;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(filter);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.PostAsync("api/Login/GetWorkOrders", content).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    WorkOrderResponse response = JsonConvert.DeserializeObject<WorkOrderResponse>(strData);
                    workOrders = response.WorkOrderList;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving Work Orders");
                }
            }
            catch (Exception ex)
            {

            }

            return workOrders;
        }

        public void AddWorkOrder(AddWorkOrderRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddWorkOrder", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(strData);

                    if (apiResponse.Messages.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (KeyValuePair<string, List<string>> messages in apiResponse.Messages)
                        {
                            foreach (string msg in messages.Value)
                            {
                                sb.AppendLine(msg);
                            }
                        }

                        //MessageBox.Show(sb.ToString());
                    }
                    else
                    {
                        //this.WorkOrderInventoryListView.ItemsSource = null;
                    }
                }
                else
                {
                    //MessageBox.Show("Error adding Work Order");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<MaterialTypeDTO> GetMaterialTypes()
        {
            List<MaterialTypeDTO> materialTypes = new List<MaterialTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse = client.GetAsync("api/Login/GetMaterialTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetMaterialTypeResponse response = JsonConvert.DeserializeObject<GetMaterialTypeResponse>(strData);
                    materialTypes = response.MaterialTypes;
                }
                else
                {
                    // MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }
            return materialTypes;
        }

        public List<FoliageTypeDTO> GetFoliageTypes()
        {
            List<FoliageTypeDTO> foliageTypes = new List<FoliageTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
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

        public List<string> GetSizeByInventoryType(long inventoryTypeId)
        {
            List<string> sizes = new List<string>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetSizeByInventoryType?inventoryTypeid=" + inventoryTypeId.ToString()).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetSizeResponse response = JsonConvert.DeserializeObject<GetSizeResponse>(strData);
                    sizes = response.Sizes;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }

            return sizes;
        }

        public GetMaterialResponse GetMaterialByType(long materialTypeId)
        {
            GetMaterialResponse materials = new GetMaterialResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetMaterialsByType?materialTypeId=" + materialTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    materials = JsonConvert.DeserializeObject<GetMaterialResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving materials");
                }
            }
            catch (Exception ex)
            {

            }

            return materials;
        }
        public List<MaterialNameDTO> GetMaterialNamesByType(long materialTypeId)
        {
            List<MaterialNameDTO> materialNameList = new List<MaterialNameDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetMaterialNamesByType?materialTypeId=" + materialTypeId.ToString()).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetMaterialNameResponse response = JsonConvert.DeserializeObject<GetMaterialNameResponse>(strData);

                    materialNameList = response.MaterialNames;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant names");
                }
            }
            catch (Exception ex)
            {

            }

            return materialNameList;
        }

        public GetFoliageResponse GetFoliageByType(long foliageTypeId)
        {
            GetFoliageResponse foliage = new GetFoliageResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetFoliageByType?foliageTypeId=" + foliageTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    foliage = JsonConvert.DeserializeObject<GetFoliageResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving materials");
                }
            }
            catch (Exception ex)
            {

            }

            return foliage;
        }

        public List<FoliageNameDTO> GetFoliageNamesByType(long foliageTypeId)
        {
            List<FoliageNameDTO> foliageNameList = new List<FoliageNameDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetFoliageNamesByType?foliageTypeId=" + foliageTypeId.ToString()).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetFoliageNameResponse response = JsonConvert.DeserializeObject<GetFoliageNameResponse>(strData);

                    foliageNameList = response.FoliageNames;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant names");
                }
            }
            catch (Exception ex)
            {

            }

            return foliageNameList;
        }

        public GetPlantResponse GetPlants()
        {
            GetPlantResponse response = new GetPlantResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetPlants").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<GetPlantResponse>(strData);
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

        public List<PlantTypeDTO> GetPlantTypes()
        {
            List<PlantTypeDTO> plantTypes = new List<PlantTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetPlantTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetPlantTypeResponse response = JsonConvert.DeserializeObject<GetPlantTypeResponse>(strData);
                    plantTypes = response.PlantTypes;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant types");
                }
            }
            catch (Exception ex)
            {

            }
            return plantTypes;
        }

        public List<PlantNameDTO> GetPlantNamesByType(long plantTypeId)
        {
            List<PlantNameDTO> plantNameList = new List<PlantNameDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetPlantNamesByType?plantTypeId=" + plantTypeId.ToString()).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetPlantNameResponse response = JsonConvert.DeserializeObject<GetPlantNameResponse>(strData);

                    plantNameList = response.PlantNames;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plant names");
                }
            }
            catch (Exception ex)
            {

            }

            return plantNameList;
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

        public List<ContainerNameDTO> GetContainerNamesByType(long containerTypeId)
        {
            List<ContainerNameDTO> containerNameList = new List<ContainerNameDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainerNamesByType?containerTypeId=" + containerTypeId.ToString()).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    containerNameList = JsonConvert.DeserializeObject<List<ContainerNameDTO>>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving container names");
                }
            }
            catch (Exception ex)
            {

            }

            return containerNameList;
        }

        public List<ContainerTypeDTO> GetContainerTypes()
        {
            List<ContainerTypeDTO> containerTypes = new List<ContainerTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainerTypes").Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    GetContainerTypeResponse response = JsonConvert.DeserializeObject<GetContainerTypeResponse>(strData);
                    containerTypes = response.ContainerTypeList;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving container types");
                }
            }
            catch (Exception ex)
            {

            }
            return containerTypes;
        }

        public GetContainerResponse GetContainersByType(long typeId)
        {
            GetContainerResponse containers = new GetContainerResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainersByType?containerTypeId=" + typeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    containers = JsonConvert.DeserializeObject<GetContainerResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {

            }

            return containers;
        }

       
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
