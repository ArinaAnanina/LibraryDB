using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Instance
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int BookId { get; set; }
        public int DepartmentId { get; set; }
        public bool Availabilities { get; set; }
        public string ReasonForLack { get; set; }
        public virtual Book Book { get; set; }
        public virtual Department Department { get; set; }
    }
    public class InstanceRequestForUpdate
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int BookId { get; set; }
        public int DepartmentId { get; set; }
        public bool Availabilities { get; set; }
        public string ReasonForLack { get; set; }
    }
    public class InstanceRequest
    {
        public int Code { get; set; }
        public int BookId { get; set; }
        public int DepartmentId { get; set; }
        public bool Availabilities { get; set; }
        public string ReasonForLack { get; set; }
    }
}
