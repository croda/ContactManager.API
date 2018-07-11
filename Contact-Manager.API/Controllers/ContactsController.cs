using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ConsoleManager.API.Models;
using ConsoleManager.API.Repository;
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
                    .Where(c => c.FirstName.ToLower().Contains(request.SearchString))
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

        //[HttpGet]
        //public IActionResult GetAll([FromQuery] string searchString, [FromQuery]int page = 1, [FromQuery] int pageSize = DEFAULT_PAGE_SIZE)
        //{
        //    var response = new QueryContactsResponse();
        //    page = page >= 1 ? page : 1;
        //    pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;
        //    pageSize = pageSize < 1 ? DEFAULT_PAGE_SIZE : pageSize;

        //    using (var session = _store.OpenSession())
        //    {
        //        searchString = searchString?.ToLowerInvariant() ?? "";
        //        var skipCount = (page - 1) * pageSize;

        //        var contacts = session.Query<Contact>()
        //            .Stats(out QueryStatistics stats)
        //            .Where(c => c.FirstName.ToLower().Contains(searchString))
        //            .Skip(skipCount)
        //            .Take(pageSize)
        //            .ToList();

        //        response.Items = contacts;
        //        response.Page = page;
        //        response.PageSize = pageSize;
        //        response.TotalCount = stats.TotalResults;
        //    }

        //    return new OkObjectResult(response);
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var contact = _repo.Get(id);
            if (contact == null)
                return new NotFoundObjectResult(contact);
            return new OkObjectResult(contact);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
                return new BadRequestResult();

            var result = _repo.Create(contact);
            return new CreatedAtRouteResult(nameof(contact.Id), contact);
        }

        // PUT api/values/5
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var isDeleted = _repo.Delete(id);

            // todo
            if (isDeleted == false)
                return new BadRequestResult();

            return new NoContentResult();
        }

        public class QueryContactsRequest
        {
            private const int DEFAULT_PAGE_SIZE = 25;
            private const int MAX_PAGE_SIZE = 100;

            private string _searchString = "";
            public string SearchString
            {
                get => _searchString;
                set => _searchString = value?.ToLowerInvariant() ?? "";
            }

            private int _page = 1;
            public int Page
            {
                get => _page;
                set => _page = value < 1 ? 1 : value;
            }

            private int _pageSize = DEFAULT_PAGE_SIZE;

            public int PageSize
            {
                get => _pageSize;
                set
                {
                    if (value > MAX_PAGE_SIZE)
                        _pageSize = MAX_PAGE_SIZE;
                    else if (value < 1)
                        _pageSize = DEFAULT_PAGE_SIZE;
                    else
                        _pageSize = value;
                }
            }

            internal int SkipCount => (Page - 1) * PageSize;
            internal int TakeCount => PageSize;
        }

        public class QueryContactsResponse
        {
            public IEnumerable<Contact> Items { get; set; } = Enumerable.Empty<Contact>();
            public int Page { get; set; }
            public int PageSize { get; set; }
            public long TotalCount { get; set; }

            public int PageCount => (int)Math.Ceiling(TotalCount / (decimal)PageSize);
        }
    }
}
