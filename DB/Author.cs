using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Author
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
        public Person Person { get; set; }
    }

    public class AuthorRequest
    {
        public int PersonId { get; set; }
        public string Description { get; set; }
    }
    public class AuthorRequestForUpdate
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
    }

    public class AuthorResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual Person Person { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
