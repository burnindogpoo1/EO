
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class CardInput
    {
        public CardInput()
        {
            junk = "sacrifice";
        }
        public string junk { get; set; }
        public string cardNumber { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string cvc { get; set; }
        public string email { get; set; }
        public string expirationDate { get; set; }
        public string nameonCard { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public decimal amount { get; set; }
        public string customerName { get; set; }

        public string token { get; set; }
    }
}
