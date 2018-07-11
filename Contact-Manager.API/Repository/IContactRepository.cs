using ConsoleManager.API.Models;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleManager.API.Repository
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        bool Create(Contact c);
        Contact Get(Guid id);
        Contact Update(Guid id, Contact c);
        bool Delete(Guid id);
        IDocumentSession OpenSession();
    }
}
