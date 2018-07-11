using System;
using ConsoleManager.API;
using ConsoleManager.API.Models;

namespace ContactManager.Tests
{
    public class Class1
    {
        public void Foo()
        {
            var contact = new Contact { FirstName = "Pece" };
            //contact.Id = Guid.NewGuid();

            var anotherContact = contact.Clone();

        }
    }
}
