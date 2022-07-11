using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.DB
{
    public class Issue
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int EmployeeId { get; set; }
        public int? ReaderId { get; set; }
        public int InstanceId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime? ReturnDate { get; set; }
        public virtual StatusType Status { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Reader Reader { get; set; }
        public virtual Instance Instance { get; set; }
    } 
    public class IssueRequest
    {
        public int StatusId { get; set; }
        public int EmployeeId { get; set; }
        public int? ReaderId { get; set; }
        public int InstanceId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime? ReturnDate { get; set; }
    } 
    public class IssueRequestForUpdate
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int EmployeeId { get; set; }
        public int? ReaderId { get; set; }
        public int InstanceId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
