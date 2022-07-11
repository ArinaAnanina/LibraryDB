using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryDB.DB;

namespace LibraryDB
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Book_Author> Book_Author { get; set; }
        public DbSet<Book_PublicationType> Book_PublicationType { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Instance> Instance { get; set; }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PublicationType> PublicationType { get; set; }
        public DbSet<PublishingHouse> PublishingHouse { get; set; }
        public DbSet<Reader> Reader { get; set; }
        public DbSet<StatusType> StatusType { get; set; }
        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Tom1", Surname = "Flivol", MiddleName = "Grander", BirthDate = DateTime.Now, Domicile = "China", PhoneNumber = "89235778178"},
                new Person { Id = 2, Name = "Tom2", Surname = "Flivol", MiddleName = "Grander", BirthDate = DateTime.Now, Domicile = "China", PhoneNumber = "89235778178"},
                new Person { Id = 3, Name = "Tom3", Surname = "Flivol", MiddleName = "Grander", BirthDate = DateTime.Now, Domicile = "China", PhoneNumber = "89235778178"}
                );
            modelBuilder.Entity<Author>().HasData(
                new Author { Id  = 1, PersonId = 1, Description = "sdfghj"}
                );
            modelBuilder.Entity<Reader>().HasData(
                new Reader { Id = 1, PersonId = 2, SeriesNumberPassport = "5715 370960"}
                );
            modelBuilder.Entity<PublishingHouse>().HasData(
                new PublishingHouse { Id = 1, Name= "Cristal", Code = 23456, Location = "Pekin"}
                );
            modelBuilder.Entity<Book>().HasData(
                new Book { Id  = 1, Name = "Grand Hotel", PublishingHouseId = 1, YearOfIssue = DateTime.Now },
                new Book { Id = 2, Name = "Frost", PublishingHouseId = 1, YearOfIssue = DateTime.Now}
                );
            modelBuilder.Entity<Book_Author>().HasData(
                new Book_Author { Id = 1, AuthorId = 1, BookId = 1},
                new Book_Author { Id = 2, AuthorId = 1, BookId = 2}
                );
            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Name = "Librarian"}
                );
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, PostId = 1, PersonId = 3, SeriesNumberPassport = "5716 904876", Seniority = 2}
                );
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction"}
                );
            modelBuilder.Entity<PublicationType>().HasData(
                new PublicationType { Id = 1, Name = "Detectives Story", CategoryId = 1}
                );
            modelBuilder.Entity<Book_PublicationType>().HasData(
                new Book_PublicationType { Id = 1, BookId = 1, PublicationTypeId = 1},
                new Book_PublicationType { Id = 2, BookId = 2, PublicationTypeId = 1}
                );
            modelBuilder.Entity<StatusType>().HasData(
                new StatusType { Id = 1, Name = "Lost"},
                new StatusType { Id = 2, Name = "OnHands"},
                new StatusType { Id = 3, Name = "InLibrary"}
                );
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Department154"},
                new Department { Id = 2, Name = "Department132"}
                );
            modelBuilder.Entity<Instance>().HasData(
                new Instance { Id  = 1, BookId = 1, DepartmentId = 1, Availabilities = true, Code = 14766},
                new Instance { Id  = 2, BookId = 1, DepartmentId = 1, Availabilities = true, Code = 14766},
                new Instance { Id  = 3, BookId = 2, DepartmentId = 2, Availabilities = false, Code = 14766, ReasonForLack = "OnHands"}
                );
            modelBuilder.Entity<Issue>().HasData(
                new Issue { Id = 1, EmployeeId = 1, ReaderId = 1, StatusId = 2, InstanceId = 3, DateOfIssue = DateTime.Now}
                );
        }

        public static string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=library;Trusted_Connection=True;";
    }
}
