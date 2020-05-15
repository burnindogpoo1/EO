using Newtonsoft.Json;
using SharedData;
using Stripe;
using System;
using System.Collections.Generic;
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
    public partial class PaymentPage : ContentPage
    {
        public decimal salePrice { get; set; }

        public string Buyer { get; set; }

        public PaymentPage(decimal salePrice, string Buyer)
        {
            InitializeComponent();

            this.salePrice = salePrice;
        }

        private void Pay_Clicked(object sender, EventArgs e)
        {
            Pay.IsEnabled = false;
                      
            CreditCard cc = new CreditCard()
            {
                Cvc = CVV.Text,
                HolderName = NameOnCard.Text,
                Numbers = CardNumber.Text,
                Month = ExpirationMonth.Text,
                Year = ExpirationYear.Text
            };

            List<string> msgs = cc.VerifyCreditCardInfo();

            if (msgs.Count == 0)
            {
                StripeServices stripe = new StripeServices();

                string wtf = stripe.CardToToken(cc).Result;

                cc.token = wtf;

                PaymentResponse response = MakeStripePayment(cc);

                if(response.success)
                {
                    //navigate back to WorkOrder and clear all fields
                    DisplayAlert("Success", "Payment Successful", "OK");
                    Task<Page> p = Navigation.PopModalAsync();

                    if (p.Result is ContentPage)
                    {
                        ContentPage cp = p.Result as ContentPage;

                        if (cp != null)
                        {
                            if (cp is WorkOrderPage)
                            {
                                //clear fields
                                ((WorkOrderPage)cp).OnClear(this, null);
                            }
                        }
                    }
                }
                else
                {
                    //add message let them try again?
                    DisplayAlert("Error", "Payment Unsuccessful", "OK");
                    Pay.IsEnabled = true;
                }
            }
            else
            {
                string errorMsg = String.Empty;
                foreach(string msg in msgs)
                {
                    errorMsg += msg + "\n";
                }

                ErrorMessages.Text = errorMsg;
                Pay.IsEnabled = true;
            }
        }

        private PaymentResponse MakeStripePayment(CreditCard creditCard)
        {
            PaymentResponse response = new PaymentResponse();

            try
            {
                CardInput cc = new CardInput();

                //cc.nameonCard = creditCard.HolderName;
                ////testing 4242424242424242
                //cc.cardNumber = "4242424242424242";
                ////cc.cardNumber = creditCard.Numbers;
                //cc.expirationDate = new DateTime(Convert.ToInt32(creditCard.Year), Convert.ToInt32(creditCard.Month), 1).ToShortDateString();
                //cc.cvc = creditCard.Cvc;
                //cc.amount = salePrice;
                //cc.customerName = "test buyer";

                //string hash = JsonConvert.SerializeObject(cc);

                //byte[] hash1 = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                //byte[] hash2 = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                
                PaymentRequest request = new PaymentRequest();

                //Random rnd = new Random();
                //request.test = rnd.Next(32, 64);

                //request.payload = Encryption.EncryptStringToBytes(hash, hash1, hash2);

                //request.payload = Encryption.EncryptStringToBytes(hash, Encryption.GetBytes(Encryption.StatsOne(request.test)), Encryption.GetBytes(Encryption.StatsTwo(request.test)));

                cc.token = creditCard.token;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(((App)App.Current).LAN_Address);

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", ((App)App.Current).User + " : " + ((App)App.Current).Pwd);

                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/MakeStripePayment", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    response = JsonConvert.DeserializeObject<PaymentResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error processing the credit card payment.");
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return response;
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            Task<Page> p = Navigation.PopModalAsync();
        }
    }
}