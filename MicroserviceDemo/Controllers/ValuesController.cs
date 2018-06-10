using System.Collections.Generic;
using Catalog.Reponsitory.Organizations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityServer.Attr;

namespace MicroserviceDemo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [HeaderAttribute]
    public class ValuesController : Controller
    {
        private readonly IMSV_OrganizationService _service;

        public ValuesController(IMSV_OrganizationService sv)
        {
            _service = sv;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Catalog 1", "Catalog 2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "values";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
