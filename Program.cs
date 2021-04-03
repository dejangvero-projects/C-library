using System;
using System.Collections.Generic;

namespace IntermediateProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var book1 = new Book("Tom Sawyer", "Mark Twain");
            var book2 = new Book("The Little Prince", " Antoine de Saint-Exupéry");

            var user1 = new User("Dejan Gvero");
            var user2 = new User("Branislava Ceran");

            var library = new Library();

            library.AddBook(book1);
            library.AddBook(book2);

            library.AddUser(user1);
            library.AddUser(user2);            

            library.Run();
        }
    }
}
