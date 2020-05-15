
using Android.Runtime;
using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetByteArrayResponse : ApiResponse
    {
        public HttpResponseMessage HttpResponse { get; set; }

        public byte[] ImageData { get; set; }

        public GetByteArrayResponse()
        {
            ImageData = new byte[0];
        }
        public GetByteArrayResponse(HttpResponseMessage httpResponse)
        {
            HttpResponse = httpResponse;
        }
    }
}
