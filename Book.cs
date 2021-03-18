using System.Threading;

namespace IntermediateProject
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }        
        public bool IsRented { get; set; }
        public User? RentedBy{ get; set; }        

        private static int _lastId = 0;

        public static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }

        public Book(string? title, string? author)
        {
            
            Id = GenerateId();
            Title = title;
            Author = author;            
            IsRented = false;            
        }


    }
}
