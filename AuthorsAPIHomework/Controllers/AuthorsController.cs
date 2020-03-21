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
    public class AuthorsController : ApiController
    {
        // GET: api/Authors
        public IHttpActionResult GetAuthors()
        {
            List<Author> authors = AuthorHelper.GetAllAuthors();
            return Ok(authors);
        }

        // GET: api/Authors/5
        public IHttpActionResult GetAuthor(int id)
        {
            Author author = AuthorHelper.FindAuthorById(id);
            if (author != null)
            {
                return Ok(author);
            }

            return Content(HttpStatusCode.NotFound, $"Author with id {id} not found");
        }

        // GET: api/authors/1/articles
        [Route("api/authors/{id}/articles")]
        public IHttpActionResult GetArticles(int id, string title="", string level="", string publishedDate="")
        {
            List<Article> articles = new List<Article>();
            try
            {
                articles = AuthorHelper.GetArticlesByAuthorId(id);

                if (!string.IsNullOrEmpty(title))
                {
                    articles = AuthorHelper.GetArticlesFilteredByTitle(id, title, articles);
                }
                if (!string.IsNullOrEmpty(level))
                {
                    articles = AuthorHelper.GetArticlesFilteredByLevel(id, level, articles);
                }
                if (!string.IsNullOrEmpty(publishedDate))
                {
                    articles = AuthorHelper.GetArticlesFilteredByPublishedDate(id, publishedDate, articles);
                }
            }
            catch(Exception)
            {
                return NotFound();
            }
            if (articles.Count==0)
            {
                return Content(HttpStatusCode.NotFound, $"Sorry, we couldn't find any results.");
            }
            
            
            return Ok(articles);
        }


        // POST: api/Authors
        public IHttpActionResult Post([FromBody] Author author)
        {
            if (!(Request.Headers.Contains("Authorization") && Request.Headers.Authorization.Scheme.Equals("admin")))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid && author != null)
            {
                try
                {
                    AuthorHelper.AddNew(author);
                    return Created("api/authors", author);
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Content(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
