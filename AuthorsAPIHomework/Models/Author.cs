using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorsAPIHomework.Models
{

    public class Author
    {
        public static int counter=1;
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username{ get; set; }
        public string Email { get; set; }
        public List<Article> Articles { get; set; }

    }
}