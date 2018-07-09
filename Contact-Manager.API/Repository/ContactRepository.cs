using ConsoleManager.API.Models;
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

        private readonly string fileName;

        private void SaveToFile(IEnumerable data)
        {
            string json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(fileName, json);
        }

        private IEnumerable<Contact> ReadFromFile()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Contact>>(System.IO.File.ReadAllText(fileName));
        }

        private void UpdateFile(IEnumerable data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            System.IO.File.WriteAllText(fileName, json);
        }

        public ContactRepository()
        {
            fileName = "db.json";

            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine("[]");
            }

        }

        public bool Create(Contact c)
        {
            var con = new Contact(c);
            List<Contact> data = ReadFromFile().ToList();
            data.Add(con);
            SaveToFile(data);
            return true;
        }

        public bool Delete(Guid id)
        {
            List<Contact> data = ReadFromFile().ToList();
            var result = data.RemoveAll(contact => contact.Id == id);
            if (result <= 0)
                return false;
            UpdateFile(data);
            return true;
        }

        public Contact Update(Guid id, Contact con)
        {
            List<Contact> data = ReadFromFile().ToList();
            int index = data.FindIndex(c => c.Id == id);
            con.Id = id;

            if (index != -1)
            {
                data[index] = con;
                UpdateFile(data);
                return con;
            }

            return null;
        }

        public IEnumerable<Contact> GetAll()
        {
            return ReadFromFile();
        }

        public Contact Get(Guid id)
        {
            var data = ReadFromFile();
            var contact = data.FirstOrDefault<Contact>(c => c.Id == id);
            if (contact != null)
                return contact;
            return null;
        }
    }
}
