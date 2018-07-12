using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ConsoleManager.API.Models;
using ConsoleManager.API.Repository;
using ContactManager.API.Queries;
using Marten;
using Marten.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleManager.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactRepository _repo;
        private static IDocumentStore _store = DocumentStore
            .For("host=localhost;port=5432;database=contact-manager-db;password=haseltintern;username=postgres");

        public ContactsController(IContactRepository repo)
        {
            _repo = repo;
        }

        // GET api/contacts
        [HttpGet]
        public IActionResult GetAll([FromQuery]QueryContactsRequest request)
        {
            request = request ?? new QueryContactsRequest();
            var response = new QueryContactsResponse();

            using (var session = _store.OpenSession())
            {
                var contacts = session.Query<Contact>()
                    .Stats(out QueryStatistics stats)
                    .Where(c => c.FirstName.Contains(request.SearchString, StringComparison.OrdinalIgnoreCase))
                    .Skip(request.SkipCount)
                    .Take(request.TakeCount)
                    .ToList();

                response.Items = contacts;
                response.Page = request.Page;
                response.PageSize = request.PageSize;
                response.TotalCount = stats.TotalResults;
            }

            return new OkObjectResult(response);
        }

        // GET api/contacts/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var contact = _repo.Get(id);
            if (contact == null)
                return new NotFoundObjectResult(contact);
            return new OkObjectResult(contact);
        }

        // POST api/contacts
        [HttpPost]
        public IActionResult Create([FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
                return new BadRequestResult();

            var result = _repo.Create(contact);
            return new CreatedAtRouteResult(nameof(contact.Id), contact);
        }

        // PUT api/contacts/5
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]Contact contact)
        {
            if (contact == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = _repo.Update(id, contact);

            if (result == null)
                return new NotFoundObjectResult(result);

            return new OkObjectResult(result);
        }

        // DELETE api/contacts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var isDeleted = _repo.Delete(id);

            // todo
            if (isDeleted == false)
                return new BadRequestResult();

            return new NoContentResult();
        }
    }
}
