using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using static System.String;


namespace IntermediateProject
{
    public class Library
    {
        private List<Book> Books { get; }
        public List<Book> AvailableBooks => Books.FindAll(b => !b.IsRented);
        public List<Book> RentedBooks => Books.FindAll(b => b.IsRented);
        private List<User> Users { get; }
        private List<Author> Authors { get; }
        private List<Rental> Rentals { get; }

        public Library()
        {
            Books = new List<Book>();
            Users = new List<User>();
            Authors = new List<Author>();
            Rentals = new List<Rental>();
        }
        public void AddBook(Book? book)
        {
            if (book != null) Books.Add(book);
        }
        public void AddUser(User? user)
        {
            if (user != null) Users.Add(user);
        }
        public void AddAuthor(Author? author)
        {
            if (author != null) Authors.Add(author);
        }
        public void RemoveBook(int? bookToRemoveId)
        {
            var bookToRemove = Books.FirstOrDefault(b => b.Id == bookToRemoveId);
            if (bookToRemove != null) Books.Remove(bookToRemove);
        }

        private int? TryGetUserId()
        {
            Console.Clear();
            GetAllUsers();
            Console.Write("Enter [Id] or [any] for back: ");
            var input = Console.ReadLine();
            if (IsNullOrEmpty(input))
            {
                return null;
            }
            var success = int.TryParse(input, out var userid);
            if (!success) return null;
            return CheckUserIdExist(userid, Users) == false ? (int?)null : userid;
        }
        private int? TryGetAuthorId()
        {
            Console.Clear();
            GetAllAuthors();
            Console.Write("Enter [Id] or [any] for back: ");
            var input = Console.ReadLine();
            if (IsNullOrEmpty(input))
            {
                return null;
            }
            var success = int.TryParse(input, out var authorId);
            if (!success) return null;
            return CheckAuthorIdExist(authorId, Authors) == false ? (int?)null : authorId;
        }
        private int? TryGetBookAuthorId(Book book)
        {
            Console.Clear();
            GetAllBookAuthors(book);
            Console.Write("Enter [Id] or [any] for back: ");
            var input = Console.ReadLine();
            if (IsNullOrEmpty(input))
            {
                return null;
            }
            var success = int.TryParse(input, out var authorId);
            if (!success) return null;
            return CheckAuthorIdExist(authorId, Authors) == false ? (int?)null : authorId;
        }


        public void RemoveUser(int? userId)
        {
            //for (var i = 0; i < Users.Count; i++)
            //{
            //    if (userId != Users[i].Id) continue;
            //    foreach (var book in Books.Where(book => book.RentedBy == Users[i]))
            //    {
            //        book.RentedBy = null;
            //        book.IsRented = false;
            //    }
            //    Users.Remove(Users[i]);
            //}
            var user = Users.FirstOrDefault(u => u.Id == userId);
            var rentedBooksByUser = Books.Where(book => book.RentedBy == user);

            foreach (var book in rentedBooksByUser)
            {
                book.RentedBy = null;
                book.IsRented = false;
            }

            if (user != null) Users.Remove(user);
        }
        public void RemoveAuthor(int? authorId)
        {
            var author = Authors.FirstOrDefault(a => a.Id == authorId);
            if (author != null) Authors.Remove(author);
        }
        public bool CheckBookIdExist(int id, List<Book> books)
        {
            return books.Any(b => b.Id == id);
        }
        public bool CheckUserIdExist(int input, List<User> users)
        {
            return users.Any(u => u.Id == input);
        }
        public bool CheckAuthorIdExist(int input, List<Author> authors)
        {
            return authors.Any(u => u.Id == input);
        }
        public bool CheckAuthorBookIdExist(int input, Book book)
        {
            var ids = book.Authors.Select(a => a.Id).ToList();
            return ids.Contains(input);
        }
        public Book? RegisterBook(string? title, List<Author>? authors)
        {
            return title != null && authors != null ? new Book(title, authors) : null;
        }
        private string? ValidateInput(string context)
        {
            Console.Clear();
            Console.Write("Enter [{0}] or [blank] for back: ", context);
            var value = Console.ReadLine().Trim();
            return IsNullOrEmpty(value) ? null : value;
        }
        private List<Author>? ValidateAuthors()
        {
            var authors = new List<Author>();
            var author = GetAuthor(TryGetAuthorId());
            if(author != null) authors.Add(author);            
            return authors.Count == 0 ? null : authors;
        }
        private Author? AppendAuthor()
        {
            return GetAuthor(TryGetAuthorId());
        }
        private Author? GetBookAuthors(Book? book)
        {
            return book != null ? GetAuthor(TryGetBookAuthorId(book)) : null;
        }
        public User? RegisterUser(string? name)
        {
            return IsNullOrEmpty(name) ? null : new User(name);
        }
        public Author? RegisterAuthor(string? name)
        {
            return IsNullOrEmpty(name) ? null : new Author(name);
        }
        public bool BooksAreRented(List<Book> books)
        {
            return books.Any(b => b.IsRented);
        }
        public void ShowBooks(List<Book> books)
        {
            var builder = new StringBuilder();
            if (books.All(b => !b.IsRented))
            {
                builder.Append($" {"ID",-4} {"TITLE",-30} {"AUTHORS", -40}\n\n");

            }
            else if (books.Any(b => b.IsRented))
            {
                builder.Append($" {"ID",-4} {"TITLE",-30} {"AUTHORS",-40} {"RENTER",-1}\n\n");
            }
            foreach (var book in books)
            {
                builder.Append(BooksAreRented(books)
                    ? $" {book.Id,-4} {book.Title,-30} {book.AllAuthors, -40} {book.RentedBy?.Name,-1}\n"
                    : $" {book.Id,-4} {book.Title,-30} {book.AllAuthors,-40}\n");
            }
            Console.WriteLine($"\n{builder}");
        }
        public void ShowRentals()
        {
            var builder = new StringBuilder();
            builder.Append($" {"ID",-4} {"BOOK TITLE",-30} {"USER RENTING",-40} {"DATE RENTED",-50}\n\n");
            if (Rentals.Count == 0)
            {
                builder.Append("\n");
                builder.Append($" {" ", -4} {" ", -25 } {"There aren't any rentals at the moment",-35} \n\n");
            }
            foreach (var rent in Rentals)
            {
                builder.Append($" {rent.Id,-4} {rent.RentedBook.Title,-30} {rent.RentedBy.Name,-40} {rent.RentingDate,-50}\n");
            }
            Console.WriteLine($"\n{builder}");
        }
        public void GetAllUsers()
        {
            var builder = new StringBuilder();
            builder.Append($" {"ID",-4} {"NAME",-20}\n\n");
            foreach (var user in Users)
            {
                builder.Append($" {user.Id,-4} {user.Name,-20}\n");
            }
            Console.WriteLine($"\n{builder}");
        }
        public void GetAllAuthors()
        {
            var builder = new StringBuilder();
            builder.Append($" {"ID",-4} {"NAME",-20}\n\n");
            foreach (var author in Authors)
            {
                builder.Append($" {author.Id,-4} {author.Name,-20}\n");
            }
            Console.WriteLine($"\n{builder}");
        }
        public void GetAllBookAuthors(Book book)
        {
            var builder = new StringBuilder();
            builder.Append($" {"ID",-4} {"NAME",-20}\n\n");
            foreach (var author in book.Authors )
            {
                builder.Append($" {author.Id,-4} {author.Name,-20}\n");
            }
            Console.WriteLine($"\n{builder}");
        }
        public Book? GetBook(int? bookId, List<Book> books)
        {
            return books.Find(b => b.Id == bookId);
        }
        public User? GetUser(int? userId)
        {
            return Users.Find(u => u.Id == userId);
        }
        public Author? GetAuthor(int? authorId)
        {
            return Authors.Find(a => a.Id == authorId);
        }
        public void RentBook(Book book, User user)
        {
            Console.WriteLine();
            if (book.IsRented)
                throw new InvalidOperationException("Can't rent rented book");
            if (book == null || user == null) return;
            user.RentedBooks.Add(book);
            book.IsRented = true;
            book.RentedBy = user;
            var rental = new Rental(book, user);
            Rentals.Add(rental);
        }

        private Rental GetRentalWithBook(Book? book)
        {
            return Rentals.FirstOrDefault(r => r.RentedBook == book);
        }

        public void ReturnBook(Book? book)
        {
            if (book != null && !book.IsRented)
                throw new InvalidOperationException("Can't return a non-rented book");
            if (book == null) return;
            book.IsRented = false;
            book.RentedBy = null;
            Rentals.Remove(GetRentalWithBook(book));
        }
        private int? ValidateBookInput(List<Book> books)
        {
            Console.Clear();
            ShowBooks(books);
            Console.Write("Enter [Id] or [any] for back: ");
            var input = Console.ReadLine();
            if (IsNullOrEmpty(input))
            {
                Console.Clear();
                return null;
            }
            var success = int.TryParse(input, out var bookId);
            Console.Clear();
            if (!success) return null;
            return CheckBookIdExist(bookId, books) == false ? (int?)null : bookId;
        }
        private void RentBook()
        {
            var user = GetUser(TryGetUserId());
            var book = GetBook(ValidateBookInput(AvailableBooks), AvailableBooks);
            if (user != null && book != null)
            {
                RentBook(book, user);
            }
        }
        private static void AddAuthorToBook(Book? book, Author? author)
        {
            if (book == null || author == null) return;
            if (!book.Authors.Contains(author)) book.Authors.Add(author);
        }

        private static void RemoveAuthorFromBook(Book? book, Author? author)
        {
            if (book == null || author == null) return;
            if (book.Authors.Contains(author)) book.Authors.Remove(author);
        }

        public void BookMenu()
        {
            while (true)
            {
                ShowBooks(Books);
                Console.Write("[1] Add Book [2] Remove Book [3] Show available Books [4] Rent Book [5] Return book [6] Add Author [7] Remove Author [any] Back: ");
                var bookInput = Console.ReadLine();
                Console.WriteLine();
                switch (bookInput)
                {
                    case "1":
                        AddBook(RegisterBook(ValidateInput("title"), ValidateAuthors()));
                        Console.Clear();
                        break;
                    case "2":
                        RemoveBook(ValidateBookInput(Books));
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        ShowBooks(AvailableBooks);
                        Console.Write("Press any key to go back");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "4":
                        RentBook();
                        Console.Clear();
                        break;
                    case "5":
                        ReturnBook(GetBook(ValidateBookInput(RentedBooks), RentedBooks));
                        break;
                    case "6":
                        AddAuthorToBook(GetBook(ValidateBookInput(Books), Books), AppendAuthor());
                        Console.Clear();
                        break;
                    case "7":
                        var book = GetBook(ValidateBookInput(Books), Books);
                        RemoveAuthorFromBook(book, GetBookAuthors(book));
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        return;
                }

            }

        }
        public void UserMenu()
        {
            while (true)
            {
                GetAllUsers();
                Console.Write("[1] Add User [2] Remove User [any] Back: ");
                var userInput = Console.ReadLine();
                Console.WriteLine();
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        AddUser(RegisterUser(ValidateInput("name")));
                        Console.Clear();
                        break;
                    case "2":
                        RemoveUser(TryGetUserId());
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        return;
                }
            }
        }
        public void AuthorMenu()
        {
            while (true)
            {
                GetAllAuthors();
                Console.Write("[1] Add Author [2] Remove Author [any] Back: ");
                var authorInput = Console.ReadLine();
                Console.WriteLine();
                switch (authorInput)
                {
                    case "1":
                        Console.Clear();
                        AddAuthor(RegisterAuthor(ValidateInput("name")));
                        Console.Clear();
                        break;
                    case "2":
                        RemoveAuthor(TryGetAuthorId());
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        return;
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("[1] Books [2] Users [3] Authors [4] Rentals [any] Exit: ");
                var input = Console.ReadLine();
                Console.WriteLine();
                switch (input)
                {
                    case "1":
                        Console.Clear();
                        BookMenu();
                        break;
                    case "2":
                        Console.Clear();
                        UserMenu();
                        break;
                    case "3":
                        Console.Clear();
                        AuthorMenu();
                        break;
                    case "4":
                        Console.Clear();
                        ShowRentals();
                        Console.Write("Press any key to go back");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    default:
                        Console.Write("Exiting");
                        Thread.Sleep(500);
                        Console.Write(".");
                        Thread.Sleep(500);
                        Console.Write(".");
                        Thread.Sleep(500);
                        Console.Write(".");
                        return;
                }
            }
        }
    }
}
