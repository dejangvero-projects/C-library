using System.Collections.Generic;
using System.Threading;

namespace IntermediateProject
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Book?> ListOfRentedBooks { get; set; }

        private static int _lastId;

        public static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }
        public User(string name)
        {            
            Id = GenerateId();
            Name = name;
            ListOfRentedBooks = new List<Book?>();
        }

    }
}
