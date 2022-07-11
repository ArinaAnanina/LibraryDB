using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class PublicationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

    public class PublicationTypeRequest
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
    public class PublicationTypeRequestForUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
    public class PublicationTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
