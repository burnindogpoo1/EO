using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Person
    {
        public Person()
        {
            PersonAddressMap = new HashSet<PersonAddressMap>();
        }

        public long PersonId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhonePrimary { get; set; }
        public string PhoneAlt { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PersonAddressMap> PersonAddressMap { get; set; }
    }
}
