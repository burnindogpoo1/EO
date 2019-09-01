using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    public class GetLongIdResponse : ApiResponse
    {
        public long returnedId {get; set;}
    }

    public class GetKvpLongStringResponse : ApiResponse
    {
        public List<KeyValuePair<long, string>> KvpList { get; set; }

        public GetKvpLongStringResponse()
        {
            KvpList = new List<KeyValuePair<long, string>>();
        }

        public GetKvpLongStringResponse(List<KeyValuePair<long, string>> kvpList)
        {
            KvpList = kvpList;
        }
    }
}
