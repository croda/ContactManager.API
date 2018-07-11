using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleManager.API.Models
{
    public class Contact
    {
        public Contact(Contact c)
        {
            Id = c.Id;
            FirstName = c.FirstName;
            LastName = c.LastName;
            Email = c.Email;
            PhoneNumber = c.PhoneNumber;
        }

        public Contact()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
