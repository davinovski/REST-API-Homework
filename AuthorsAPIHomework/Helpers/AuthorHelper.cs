using AuthorsAPIHomework.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AuthorsAPIHomework.Helpers
{
    public static class AuthorHelper
    {
        public static List<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();
            using (StreamReader r = new StreamReader(GetFilePath()))
            {
                string json = r.ReadToEnd();
                authors = JsonConvert.DeserializeObject<List<Author>>(json);
            }
            return authors;
        }
        
        public static Author FindAuthorById(int id)
        {
            List<Author> authors = GetAllAuthors();
            Author author = authors.FindLast(a => a.UserId == id);
            return author;
        }

        public static List<Article> GetArticlesByAuthorId(int id)
        {
            List<Author> authors = GetAllAuthors();
            Author author = authors.FindLast(a => a.UserId == id);
            return author.Articles;
        }

        public static List<Article> GetArticlesFilteredByTitle(int id, string title, List<Article> articles)
        {
            List<Article> arts = articles.Where(a => a.Title.ToLower().Contains(title.ToLower())).ToList();
            return arts;
        }

        public static List<Article> GetArticlesFilteredByLevel(int id, string level, List<Article> articles)
        {
            List<Article> arts = articles.Where(a => a.Level.ToString().ToLower().Equals(level.ToLower())).ToList();
            return arts;
        }

        public static List<Article> GetArticlesFilteredByPublishedDate(int id, string publishedDate, List<Article> articles)
        {
            List<Article> arts = articles.Where(a => a.DatePublished.Equals(publishedDate)).ToList();
            return arts;
        }

        public static void AddNew(Author author)
        {
            var filePath = GetFilePath();
            List<Author> allAuthors = GetAllAuthors();
            author.UserId = allAuthors.Count()+1;
            if (allAuthors.Any(a => a.UserId == author.UserId))
            {
                throw new Exception();
            }
            foreach(Article article in author.Articles)
            {
                article.Id = GenerateId();
            }
            allAuthors.Add(author);

            File.WriteAllText(filePath, JsonConvert.SerializeObject(allAuthors));
        }

        public static void AddNewArticle(int id, Article article)
        {
            string filePath = GetFilePath();
            var allAuthors = GetAllAuthors();
            var author = FindAuthorById(id);
            var index = allAuthors.FindIndex(a=>a.UserId==id);

            author.Articles.Add(article);

            if (index != -1)
            {
                allAuthors[index] = author;
            }

            File.WriteAllText(filePath, JsonConvert.SerializeObject(allAuthors));
        }
        public static List<string> AllAuthorizedUsernames()
        {
            List<Author> authors = GetAllAuthors();
            return authors.Select(author => author.Username).ToList();
        }

        public static void Update(int userId, string id, Article article)
        {
            string filePath = GetFilePath();
            var allAuthors = GetAllAuthors();
            var author = FindAuthorById(userId);
            var index = allAuthors.FindIndex(a => a.UserId == userId);
            var indexAuthor = author.Articles.FindIndex(a => a.Id.Equals(id));

            Article art = author.Articles[indexAuthor];
            author.Articles.RemoveAt(indexAuthor);

            art.Title = article.Title;
            art.Level = article.Level;

            author.Articles.Add(art);

            if (index != -1)
            {
                allAuthors[index] = author;
            }

            File.WriteAllText(filePath, JsonConvert.SerializeObject(allAuthors));

            /*List<Author> auths;

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                auths = JsonConvert.DeserializeObject<List<Author>>(json);
                auths.Where(a => a.UserId == userId).Select(x =>
                {
                    x.Articles[x.Articles.FindIndex(art => art.Id.Equals(id))] = (Article) x.Articles.Where(aa => aa.Id.Equals(id)).Select(articlee =>
                        {
                            articlee.Title = article.Title;
                            articlee.Level = article.Level;
                            return articlee;
                        });

                    return x;

                }).ToList();
            }*/


        }

        public static string GenerateId()
        {
            int length = 15;
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }



        private static string GetFilePath()
        {
            return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data", "Authors.json");
        }
        
    }
}