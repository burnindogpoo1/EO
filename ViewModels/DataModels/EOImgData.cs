using Android.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModels.DataModels
{

    [Serializable]
    [Preserve(AllMembers = true)]
    public class EOImgData
    {
        public EOImgData()
        {
            isNewImage = true;
        }
        public byte[] imgData { get; set; }

        public string fileName { get; set; }

        public bool isNewImage { get; set; }
    }
}
