using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IntermediateProject
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsRented { get; set; }
        public User? RentedBy{ get; set; }
        public List<Author> Authors { get; set; }

        public string AllAuthors
        {
            get
            {
                return Authors.Aggregate("", (current, author) => current + author.Name + ", ").TrimEnd(',', ' ');
            }
        }

        private static int _lastId;

        private static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }

        public Book(string title, List<Author> authors)
        {
            Id = GenerateId();
            Title = title;
            Authors = authors;
        }


    }
}
