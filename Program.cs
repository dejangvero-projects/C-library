using System;
using System.Collections.Generic;

namespace IntermediateProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var library = new Library();

            var author1 = new Author("Mark Twein");
            var author2 = new Author("Tourist");
            var author3 = new Author("Antoine de Saint-Exupéry");

            var book1 = new Book("Tom Sawyer", authors: new List<Author> { author1 , author2});
            var book2 = new Book("The Little Prince", new List<Author> { author3 });

            var user1 = new User("Dejan Gvero");
            var user2 = new User("Branislava Ceran");


            library.AddAuthor(author1);
            library.AddAuthor(author2);
            library.AddAuthor(author3);

            library.AddBook(book1);
            library.AddBook(book2);

            library.AddUser(user1);
            library.AddUser(user2);

            Console.SetWindowSize(160, 40);
            library.Run();
        }
    }
}
