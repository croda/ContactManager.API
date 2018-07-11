using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ConsoleManager.API.Models;
using ConsoleManager.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleManager.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        public int DefaultPageRecordCount = 5;
        private readonly IContactRepository _repo;
        private QueryContactsResponse _response;

        public class QueryContactsResponse
        {
            public IEnumerable<Contact> Items { get; set; } = Enumerable.Empty<Contact>();
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int PageCount { get; set; }
            public int TotalCount { get; set; }
        }

        public ContactsController(IContactRepository repo)
        {
            _repo = repo;
            _response = new QueryContactsResponse();
        }

        // GET api/contacts
        [HttpGet]
        public IActionResult GetAll([FromQuery] string searchString, [FromQuery]int? page, [FromQuery] int? count)
        {
            var ss = searchString ?? "";
            var lowerSearchString = ss.ToLower();
            var takePage = page ?? 1;
            var takeCount = count ?? DefaultPageRecordCount; // default = 5
            var contacts = _repo.GetAll()
                                 .Where(c => c.FirstName.ToLower().Contains(lowerSearchString))
                                 .Skip((takePage - 1) * takeCount)
                                 .Take(takeCount)
                                 .ToList();

            var totalItems = _repo.GetAll().Where(c => c.FirstName.Contains(lowerSearchString)).ToList().Count;
            decimal pageCount = totalItems / (decimal)takeCount;

            _response.Items = contacts;
            _response.Page = takePage;
            _response.PageSize = contacts.Count;
            _response.TotalCount = totalItems;
            _response.PageCount = (int)Math.Ceiling(pageCount);

            return new OkObjectResult(_response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var c = _repo.Get(id);
            if (c == null)
                return new NotFoundObjectResult(c);
            return new OkObjectResult(c);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody]Contact c)
        {
            if (!ModelState.IsValid)
                return new BadRequestResult();

            var result = _repo.Create(c);
            return new CreatedAtRouteResult(nameof(c.Id), c);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]Contact c)
        {
            if (c == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = _repo.Update(id, c);

            if (result == null)
                return new NotFoundObjectResult(result);

            return new OkObjectResult(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _repo.Delete(id);

            if (result == null)
                return new BadRequestResult();

            return new NoContentResult();
        }
    }
}
