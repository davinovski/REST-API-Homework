using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorsAPIHomework.Models
{
    public enum Level
    {
        beginner,
        intermediate,
        advanced
    }
    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string DatePublished { get; set; }
        public Level Level { get; set; }

    }
}