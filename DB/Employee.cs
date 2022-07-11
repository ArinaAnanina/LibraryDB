using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Employee
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PostId { get; set; }
        public string SeriesNumberPassport { get; set; }
        public int Seniority { get; set; }
        public virtual Person Person { get; set; }
        public virtual Post Post { get; set; }
    }
    public class EmployeeRequest
    {
        public int PersonId { get; set; }
        public int PostId { get; set; }
        public string SeriesNumberPassport { get; set; }
        public int Seniority { get; set; }
    }
    public class EmployeeRequestForUpdate
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PostId { get; set; }
        public string SeriesNumberPassport { get; set; }
        public int Seniority { get; set; }
    }
}
