using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static System.String;


namespace IntermediateProject
{
    public class Library
    {
        private List<Book> Books { get;}
        public List<Book> ListOfAvailableBooks => Books.FindAll(b => b.IsRented == false);
        public List<Book> ListOfRentedBooks => Books.FindAll(b => b.IsRented);       
        private List<User> Users { get;}       

        public Library()
        {
            Books = new List<Book>();
            Users = new List<User>();
        }
        public void AddBook(Book book)
        {
            Books.Add(book);
        }
        public void AddUser(User? user)
        {
            if (user != null) Users.Add(user);
        }
        public void RemoveBook(int? bookToRemoveId)
        {
            var bookToRemove = Books.FirstOrDefault(b => b.Id == bookToRemoveId);
            Books.Remove(bookToRemove);
        }

        private int? ValidateUserId()
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
            return CheckUserIdExist(userid, Users) == false ? (int?) null : userid;
        }

        public void RemoveUser(int? userId)
        {
            for (var i = 0; i < Users.Count; i++)
            {
                if (userId != Users[i].Id) continue;
                foreach (var book in Books.Where(book => book.RentedBy == Users[i]))
                {
                    book.RentedBy = null;
                    book.IsRented = false;
                }
                Users.Remove(Users[i]);
            }
        }
        public bool CheckBookIdExist(int id, List<Book> books)
        {            
            return books.Any(b => b.Id == id);
        }
        public bool CheckUserIdExist(int input, List<User> users)
        {
            return users.Any(u => u.Id == input);
        }
        public Book RegisterBook(string? title, string? author)
        {
            return new Book(title, author);
        }
        private string? ValidateInput(string context)
        {
            Console.Write("Enter [{0}] or [blank] for back: ", context);
            var value = Console.ReadLine().Trim();
            return IsNullOrEmpty(value) ? null : value;
        }
        public User? RegisterUser(string? name)
        {
            return IsNullOrEmpty(name)? null: new User(name, new List<Book?>());
        }
        public bool BooksAreRented(List<Book> books)
        {
            return books.Any(b => b.IsRented);
        }        
        public void ShowBooks(List<Book> books)
        {
            var builder = new StringBuilder();
            if (books.All(b => b.IsRented == false))
            {
                builder.Append($" {"ID",-4} {"TITLE",-30}\n\n");

            }
            else if (books.Any(b => b.IsRented))
            {
                builder.Append($" {"ID",-4} {"TITLE",-30} {"RENTER",-1}\n\n");
            }
            foreach (var book in books)
            {
                builder.Append(BooksAreRented(books)
                    ? $" {book.Id,-4} {book.Title,-30} {book.RentedBy?.Name,-1}\n"
                    : $" {book.Id,-4} {book.Title,-30}\n");
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
        public Book? GetBook(int? bookId, List<Book> books)
        {
            return books.Find(b => b.Id == bookId);
        }
        public User GetUser(int? userId)
        {
            return Users.Find(u => u.Id == userId)!;
        }
        public void RentBook(Book? book, User? user)
        {            
            Console.WriteLine();            
            if (book != null && book.IsRented)
                throw new InvalidOperationException("Can't rent rented book");
            if (book == null || user == null) return;
            user.ListOfRentedBooks.Add(book);
            book.IsRented = true;
            book.RentedBy = user;
        }
        public void ReturnBook(Book? book)
        {
            Console.WriteLine();
            if (book != null && book.IsRented == false)
                throw new InvalidOperationException("Can't return a non-rented book");
            if (book == null) return;
            book.IsRented = false;
            book.RentedBy = null;
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
        public void BookMenu()
        {
            while (true)
            {
                ShowBooks(Books);
                Console.Write("[1] Add Book [2] Remove Book [3] Show available Books [4] Rent Book [5] Return book [any] Back: ");
                var bookInput = Console.ReadLine();
                Console.WriteLine();
                switch (bookInput)
                {
                    case "1":
                        AddBook(RegisterBook(ValidateInput("title"), ValidateInput("author")));
                        Console.Clear();
                        break;
                    case "2":
                        RemoveBook(ValidateBookInput(Books));
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        ShowBooks(ListOfAvailableBooks);
                        Console.Write("Press any key to go back");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "4":
                        RentBook(GetBook(ValidateBookInput(ListOfAvailableBooks), ListOfAvailableBooks), GetUser(ValidateUserId()));
                        Console.Clear();
                        break;
                    case "5":
                        ReturnBook(GetBook(ValidateBookInput(ListOfRentedBooks), ListOfRentedBooks));
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
                        RemoveUser(ValidateUserId());
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
                Console.Write("[1] Books [2] Users [any] Exit: ");
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
