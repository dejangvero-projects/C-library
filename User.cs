using System.Collections.Generic;
using System.Threading;

namespace IntermediateProject
{
    public class User : Person
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public List<Book?> RentedBooks { get; set; }

        private static int _lastId;

        private static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }

        public User(string name)
        {
            Id = GenerateId();
            Name = name;
            RentedBooks = new List<Book?>();
        }

        
    }
    
}
