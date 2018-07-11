using ConsoleManager.API.Models;
using Marten;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleManager.API.Repository
{
    public class ContactRepository : IContactRepository
    {

        IDocumentStore store;

        public ContactRepository()
        {
            store = DocumentStore
                .For("host=localhost;port=5432;database=contact-manager-db;password=haseltintern;username=postgres");

        }

        public bool Create(Contact c)
        {
            using (var session = store.OpenSession())
            {
                session.Store(c);
                session.SaveChanges();
                return true;
            }
        }

        public bool Delete(Guid id)
        {
            using (var session = store.OpenSession())
            {
                session.DeleteWhere<Contact>(c => c.Id == id);
                session.SaveChanges();  
                return true;
            }
        }

        public Contact Update(Guid id, Contact con)
        {
            using (var session = store.OpenSession())
            {
                var contact = session.Query<Contact>().FirstOrDefault(c => c.Id == id);
                if (contact == null)
                    return null;

                contact.FirstName = con.FirstName;
                contact.LastName = con.LastName;
                contact.Email = con.Email;
                contact.PhoneNumber = con.PhoneNumber;

                session.Store(contact);
                session.SaveChanges();
                return contact;
            }
        }

        public IEnumerable<Contact> GetAll()
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Contact>().ToList();
            }
        }

        public Contact Get(Guid id)
        {
            using (var session = store.OpenSession())
            {
                var user = session.Query<Contact>().FirstOrDefault(c => c.Id == id);
                if (user == null)
                    return null;
                return user;
            }
        }
    }
}
