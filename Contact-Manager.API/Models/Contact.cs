using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleManager.API.Models
{
    public class Contact
    {
        public Guid Id { get; set; }/* = Guid.NewGuid();*/
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //public Contact Clone()
        //{
        //    return new Contact
        //    {
        //        Id = Id,
        //        Email = Email,
        //        FirstName = FirstName,
        //        LastName = LastName,
        //        PhoneNumber = PhoneNumber
        //    };
        //}
    }
}
