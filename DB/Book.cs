using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Book
    {
        public int Id { get; set; }
        public int PublishingHouseId { get; set; }
        public string Name { get; set; }
        public DateTime YearOfIssue { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
    }

    public class BookRequest
    {
        public int PublishingHouseId { get; set; }
        public string Name { get; set; }
        public DateTime YearOfIssue { get; set; }
    }
    public class BookRequestForUpdate
    {
        public int Id { get; set; }
        public int PublishingHouseId { get; set; }
        public string Name { get; set; }
        public DateTime YearOfIssue { get; set; }
    }

    public class BookResponse
    {
        public int Id { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
        public string Name { get; set; }
        public DateTime YearOfIssue { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<PublicationType> PublicationTypes { get; set; }
    }
}
