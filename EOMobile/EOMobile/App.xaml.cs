using EO.ViewModels.ControllerModels;
using EOMobile.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EOMobile
{
    public partial class App : Xamarin.Forms.Application
    {
        //Static variables for the app
        public static string EOImageId
        {
            get { 
                string dt = DateTime.Now.ToShortDateString();
                dt = dt.Replace('/', '-');

                return "EOImage_" + dt + "_";   
            }
            set { }
        }

        public static string DefaultImageId = "defaultImage";

        public static string ImageIdToSave = null;

        //Publishable key = pk_test_qEqBdPz6WTh3CNdcc9bgFXpz00haS1e8hC
        //Secret key = sk_test_6vJyMV6NxHArGV6kI2EL6R7V00kzjXJ72u
        //stripe emrgency security bypass code = dvbb-omgm-gpro-rrlv-lcoj
        public string LAN_Address { get; set; }

        public string User { get; set; }

        public string Pwd { get; set; }

        public ArrangementInventoryDTO searchedForArrangementInventory { get; set; }

        public ShipmentInventoryItemDTO searchedForShipmentInventory { get; set; }

        public WorkOrderInventoryItemDTO searchedForInventory { get; set; }

        public PersonAndAddressDTO searchedForPerson { get; set; }

        public static List<EOImgData> imageDataList = new List<EOImgData>();

        List<string> pngFileNames;

        public App()
        {
            InitializeComponent();

            pngFileNames = new List<string>();

            LAN_Address = "http://192.168.0.129:9000/";   //Me

            //LAN_Address = "http://eo.hopto.org:9000/";   //Me

            //LAN_Address = "http://192.168.1.134:9000/";  //Thom

            //Stripe.StripeConfiguration.ApiKey = "sk_test_6vJyMV6NxHArGV6kI2EL6R7V00kzjXJ72u";

            MainPage = new NavigationPage(new LoginPage());

            MessagingCenter.Subscribe<ArrangementInventoryDTO>(this, "SearchArrangementInventory", (arg) =>
            {
                LoadArrangementInventory(arg);
            });

            MessagingCenter.Subscribe<ShipmentInventoryItemDTO>(this, "SearchShipmentInventory", (arg) =>
            {
                LoadShipmentInventory(arg);
            });

            MessagingCenter.Subscribe<WorkOrderInventoryItemDTO>(this, "SearchInventory", (arg) =>
            {
                LoadInventory(arg);
            });

            MessagingCenter.Subscribe<PersonAndAddressDTO>(this, "SearchCustomer", (arg) =>
            {
                LoadCustomer(arg);
            });

            MessagingCenter.Subscribe<string>(this, "ImageSelected", async (arg) =>
            {
                PictureSelected(arg);
            });

            MessagingCenter.Subscribe<EOImgData>(this, "ImageSaved", async (arg) =>
            {
                PictureTaken(arg);
            });

            //MessagingCenter.Subscribe<string>(this, "WTF", async (arg) =>
            //{
            //    WTFSelected(arg);
            //});

            //MessagingCenter.Subscribe<string>(this, "XXX", async (arg) =>
            //{
            //    XXXSelected(arg);
            //});
        }

        public void LoadArrangementInventory(ArrangementInventoryDTO arg)
        {
            searchedForArrangementInventory = arg;
        }

        public void LoadShipmentInventory(ShipmentInventoryItemDTO arg)
        {
            searchedForShipmentInventory = arg;
        }

        public void LoadInventory(WorkOrderInventoryItemDTO arg)
        {
            searchedForInventory = arg;
        }

        public void LoadCustomer(PersonAndAddressDTO arg)
        {
            searchedForPerson = arg;
        }

        public void PictureSelected(string arg)
        {
            pngFileNames.Add(arg);
        }

        public List<EOImgData> GetImageData()
        {
            return imageDataList;
        }

        public void ClearImageData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<ICameraInterface>().DeleteImageFromStorage(imageDataList);
            });
        }
        public void PictureTaken(EOImgData arg)
        {
            imageDataList.Add(arg);

            //MessagingCenter.Send<EOImgData>(arg, "UpdateImageList");
        }

        public void WTFSelected(string arg)
        {
            int debug = 1;
        }

        public void XXXSelected(string arg)
        {
            int debug = 1;
        }

        public void ClearImageDataList()
        {
            imageDataList.Clear();
        }

        public List<string> GetSizeByInventoryType(long inventoryTypeId)
        {
            List<string> sizes = new List<string>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(((App)App.Current).LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetSizeByInventoryType?inventoryTypeId=" + inventoryTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();

                    GetSizeResponse sizeResponse = JsonConvert.DeserializeObject<GetSizeResponse>(strData);
                    sizes = sizeResponse.Sizes;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving plants");
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return sizes;
        }

        public GetArrangementResponse GetArrangement(long arrangementId)
        {
            GetArrangementResponse response = new GetArrangementResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetArrangement?arrangementId=" + arrangementId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    response = JsonConvert.DeserializeObject<GetArrangementResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving arrangements");
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return response;
        }

        public List<GetSimpleArrangementResponse> GetArrangements(string arrangementName)
        {
            //List<ArrangementInventoryDTO> arrangements = new List<ArrangementInventoryDTO>();

            List<GetSimpleArrangementResponse> arrangements = new List<GetSimpleArrangementResponse>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetArrangements?arrangementName=" + arrangementName).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    List<GetSimpleArrangementResponse> response = JsonConvert.DeserializeObject<List<GetSimpleArrangementResponse>>(strData);

                    //arrangements = response.ArrangementList;
                    arrangements = response;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving arrangements");
                }
            }
            catch(Exception ex)
            {
                int debug = 1;
            }

            return arrangements;
        }

        public EOImgData GetImage(long imageId)
        {
            EOImgData imageData = new EOImgData();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetImage?imageId=" + Convert.ToString(imageId)).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    imageData.imgData =  httpResponse.Content.ReadAsByteArrayAsync().Result;
                    imageData.isNewImage = false;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return imageData;
        }

        public List<InventoryTypeDTO> GetInventoryTypes()
        {
            List<InventoryTypeDTO> dtoList = new List<InventoryTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);

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
                client.BaseAddress = new Uri(LAN_Address);
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
                    //MessageBox.Show("There was an error retreiving people");
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
                client.BaseAddress = new Uri(LAN_Address);
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

        public long AddShipment(AddShipmentRequest request)
        {
            long newShipmentId = 0;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddShipment", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(strData);

                    newShipmentId = apiResponse.Id;

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

            return newShipmentId;
        }

        public ShipmentInventoryDTO GetShipment(long shipmentId)
        {
            ShipmentInventoryDTO response = new ShipmentInventoryDTO();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                //string jsonData = JsonConvert.SerializeObject(filter);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetShipment?shipmentId=" + Convert.ToString(shipmentId)).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    response = JsonConvert.DeserializeObject<ShipmentInventoryDTO>(strData);
                }
            }
            catch(Exception ex)
            {

            }

            return response;
        }

        public List<ShipmentInventoryDTO> GetShipments(ShipmentFilter filter)
        {
            List<ShipmentInventoryDTO> shipments = new List<ShipmentInventoryDTO>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(filter);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.PostAsync("api/Login/GetShipments", content).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    GetShipmentResponse response = JsonConvert.DeserializeObject<GetShipmentResponse>(strData);
                    shipments = response.ShipmentList;
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving Work Orders");
                }
            }
            catch (Exception ex)
            {

            }

            return shipments;
        }

        public WorkOrderInventoryDTO GetWorkOrder(long workOrderId)
        {
            WorkOrderInventoryDTO response = new WorkOrderInventoryDTO();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                //string jsonData = JsonConvert.SerializeObject(filter);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetWorkOrder?workOrderId=" + Convert.ToString(workOrderId)).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    response = JsonConvert.DeserializeObject<WorkOrderInventoryDTO>(strData);
                }
            }
            catch (Exception ex)
            {

            }

            return response;
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
                client.BaseAddress = new Uri(LAN_Address);
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
                    //strReader.Close();
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

        public long AddWorkOrder(AddWorkOrderRequest request)
        {
            long newWorkOrderId = 0;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddWorkOrder", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(strData);
                    newWorkOrderId = response.Id;

                    //ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(strData);

                    //if (apiResponse.Messages.Count > 0)
                    //{
                    //    StringBuilder sb = new StringBuilder();
                    //    foreach (KeyValuePair<string, List<string>> messages in apiResponse.Messages)
                    //    {
                    //        foreach (string msg in messages.Value)
                    //        {
                    //            sb.AppendLine(msg);
                    //        }
                    //    }

                    //    //MessageBox.Show(sb.ToString());
                    //}
                    //else
                    //{
                    //    //this.WorkOrderInventoryListView.ItemsSource = null;
                    //}
                }
                else
                {
                    //MessageBox.Show("Error adding Work Order");
                }
            }
            catch (Exception ex)
            {

            }

            return newWorkOrderId;
        }

        public long AddWorkOrderPayment(WorkOrderPaymentDTO  request)
        {
            long newWorkOrderPaymentId = 0;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddWorkOrderPayment", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(strData);
                    newWorkOrderPaymentId = response.Id;
                }
                else
                {
                    //MessageBox.Show("Error adding Work Order");
                }
            }
            catch (Exception ex)
            {

            }

            return newWorkOrderPaymentId;
        }

        public WorkOrderPaymentDTO GetWorkOrderPayment(long workOrderId)
        {
            WorkOrderPaymentDTO workOrderPayment = new WorkOrderPaymentDTO();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetWorkOrderPayment?workOrderId=" + workOrderId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
                    workOrderPayment = JsonConvert.DeserializeObject<WorkOrderPaymentDTO>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving materials");
                }
            }
            catch (Exception ex)
            {

            }

            return workOrderPayment;
        }

        public void AddWorkOrderImage(AddWorkOrderImageRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddWorkOrderImage", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    int debug = 1;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public long AddArrangement(AddArrangementRequest request)
        {
            long newArrangementId = 0;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddArrangement", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(strData);
                    newArrangementId = response.Id;
                }
            }
            catch (Exception ex)
            {

            }

            return newArrangementId;
        }

        public void AddArrangementImage(AddArrangementImageRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/AddArrangementImage", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    int debug = 1;
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }
        }

        public long UpdateArrangement(UpdateArrangementRequest request)
        {
            long arrangementId = 0;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/UpdateArrangement", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string strData = httpResponse.Content.ReadAsStringAsync().Result;
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(strData);
                    arrangementId = response.Id;
                }
            }
            catch (Exception ex)
            {

            }

            return arrangementId;
        }

        public bool DeleteArrangement(long arrangementId)
        {
            bool arrangementDeleted = false;

            return arrangementDeleted;
        }

        public List<MaterialTypeDTO> GetMaterialTypes()
        {
            List<MaterialTypeDTO> materialTypes = new List<MaterialTypeDTO>();

            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
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
        public GetMaterialResponse GetMaterialByType(long materialTypeId)
        {
            GetMaterialResponse materials = new GetMaterialResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetMaterialsByType?materialTypeId=" + materialTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetFoliageByType?foliageTypeId=" + foliageTypeId).Result;
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetPlantsByType?plantTypeId=" + plantTypeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
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
                client.BaseAddress = new Uri(LAN_Address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", User + " : " + Pwd);

                HttpResponseMessage httpResponse =
                    client.GetAsync("api/Login/GetContainersByType?containerTypeId=" + typeId).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    //strReader.Close();
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
