using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Book_PublicationType
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PublicationTypeId { get; set; }
        public Book Book { get; set; }
        public PublicationType PublicationType { get; set; }
}
}
