using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDB.Controllers;
using LibraryDB.DB;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LibraryDB.Exceptions;

namespace LibraryDB.Services
{
    public class IssueService
    {
        ApplicationContext _context;
        private readonly ILogger<IssueController> _logger;

        DbSet<Issue> issueSet
        {
            get
            {
                return _context?.Issue;
            }
        }
        public IssueService(ILogger<IssueController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addIssue(Issue newIssue)
        {
            EntityEntry<Issue> entry = await issueSet.AddAsync(newIssue);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Issue>> getIssue()
        {
            return issueSet
                .Include(o => o.Status)
                .Include(o => o.Employee)
                .ThenInclude(b => b.Person)
                .Include(o => o.Employee)
                .ThenInclude(b => b.Post)
                .Include(o => o.Reader)
                .ThenInclude(b => b.Person)
                .Include(o => o.Instance)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Issue> getIssue(int id)
        {
            Issue result = await issueSet
                .Where(o => o.Id == id)
                .Include(o => o.Status)
                .Include(o => o.Employee)
                .ThenInclude(b => b.Person)
                .Include(o => o.Employee)
                .ThenInclude(b => b.Post)
                .Include(o => o.Reader)
                .ThenInclude(b => b.Person)
                .Include(o => o.Instance)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Issue", id);
            }

            return result;
        }
        public async Task updateIssue(IssueRequestForUpdate issue)
        {
            if (issue.StatusId <= 0 || issue.EmployeeId <= 0 || issue.ReaderId  <= 0 || issue.InstanceId <= 0 )
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Issue tmp = issueSet.Find(issue.Id);
            tmp.StatusId = issue.StatusId;
            tmp.EmployeeId = issue.EmployeeId;
            tmp.ReaderId = issue.ReaderId;
            tmp.InstanceId = issue.InstanceId;
            tmp.DateOfIssue = issue.DateOfIssue;
            tmp.ReturnDate = issue.ReturnDate;

            await _context.SaveChangesAsync();
        }
        public async Task removeIssue(int? id)
        {
            Issue issue = issueSet.Where(o => o.Id == id).FirstOrDefault();
            if (issue == null)
            {
                throw new NotFoundException("Category", id);
            }
            EntityEntry<Issue> entry = issueSet.Remove(issue);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
