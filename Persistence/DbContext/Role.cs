using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public long RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
