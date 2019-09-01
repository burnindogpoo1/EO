using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class User
    {
        public User()
        {
            Person = new HashSet<Person>();
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}
