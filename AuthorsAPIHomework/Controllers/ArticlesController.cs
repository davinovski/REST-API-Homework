using AuthorsAPIHomework.Helpers;
using AuthorsAPIHomework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthorsAPIHomework.Controllers
{
    public class ArticlesController : ApiController
    {
        // POST: api/1/articles
        [Route("api/{id}/articles")]
        public IHttpActionResult Post(int id, [FromBody] Article article)
        {
            List<string> usernames = AuthorHelper.AllAuthorizedUsernames();
            if (!(Request.Headers.Contains("Authorization") && usernames.Contains(Request.Headers.Authorization.Scheme)))
            {
                return Unauthorized();
            }

            article.Id = AuthorHelper.GenerateId();
            if (ModelState.IsValid && article != null)
            {
                try
                {
                    AuthorHelper.AddNewArticle(id, article);
                    return Created("api/authors", article);
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Content(HttpStatusCode.BadRequest, ModelState);
        }


        // PUT: api/1/articles/ddc82058-23324-4926-9314-00faff10ec71
        [Route("api/{userId}/articles/{id}")]
        public IHttpActionResult Put(int userId, string id, Article article)
        {
            List<string> usernames = AuthorHelper.AllAuthorizedUsernames();
            if (!(Request.Headers.Contains("Authorization") && usernames.Contains(Request.Headers.Authorization.Scheme)))
            {
                return Unauthorized();
            }
            try
            {
                AuthorHelper.Update(userId, id, article);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.OK);


        }
    }
}
