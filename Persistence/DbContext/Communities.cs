using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Communities
    {
        public long CommunityId { get; set; }
        public string CommunityName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
