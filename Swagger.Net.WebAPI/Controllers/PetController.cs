﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swagger.Net.WebAPI.Models;

namespace Swagger.Net.WebAPI.Controllers
{
    /// <summary>
    ///     A WebAPI example that uses HttpResponseMessage for return values along with custom routes
    ///     to better cover Swagger's interop with WebAPI
    /// </summary>
    /// <remarks>
    ///   See http://stackoverflow.com/questions/10660721/what-is-the-difference-between-httpresponsemessage-and-httpresponseexception
    ///   for a discussion on the merits of using HttpResponseMessage over other return types.
    /// </remarks>
    public class PetController : ApiController
    {
        /// <summary>
        ///     Retuns a paged list of pet names
        ///     GET api/Pet
        /// </summary>
        /// <returns cref="Pet" type="Pet[]"></returns>
        public IEnumerable<Pet> Get(int page = 1, int? size=10) {
            return new[] {
                new Pet {
                    Id = 1,
                    Name = "Pet #1",
                    Category = new Category {Id = 1, Name = "sold"},
                    PhotoUrls = new List<string> {"http://foo.com/photo/1", "http://foo.com/photo/2"},
                    Tags = new List<Tag> {new Tag {Id = 1, Name = "tag-1"}}
                },
                new Pet {Id = 2, Name = "Pet #2"}
            };
        }

        // GET api/Pet/5
        public Pet Get(int id)
        {
            return new Pet { Id=1, Name="Pet #1"};
        }

        // GET api/Pet/5
        [HttpGet]
        [ActionName("Export")]
        public HttpResponseMessage GetExport(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, String.Concat("This is an exported file for id #", id));
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Pet value)
        {
            // addd Location: header with the Id value
            return Request.CreateResponse(HttpStatusCode.Created); // 201
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]Pet value)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);

        }

        // PUT api/<controller>/5
        [HttpPut]
        [ActionName("PutExport")]
        public HttpResponseMessage Put(int id, int? userId = null)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}