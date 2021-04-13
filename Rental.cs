using System;
using System.Threading;

namespace IntermediateProject
{
    public class Rental
    {
        public int Id { get; set; }
        public Book RentedBook { get; set; }
        public User RentedBy { get; set; }
        public DateTime RentingDate { get; set; }
        private static int _lastId;

        private static int GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }
        public Rental(Book book, User user)
        {
            Id = GenerateId();
            RentedBook = book;
            RentedBy = user;
            RentingDate = DateTime.Now;
        }
    }
}