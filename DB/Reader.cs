using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Reader 
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string SeriesNumberPassport { get; set; }
        public virtual Person Person { get; set; }
    }
    public class ReaderRequest
    {
        public int PersonId { get; set; }
        public string SeriesNumberPassport { get; set; }
    }
    public class ReaderRequestForUpdate
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string SeriesNumberPassport { get; set; }
    }
}
