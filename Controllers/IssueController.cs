using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryDB.DB;
using LibraryDB.Services;

namespace LibraryDB.Controllers
{
    /// <summary>
    /// Контроллер для работы с выдачами книг
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class IssueController : ControllerBase
    {
        private readonly ILogger<IssueController> logger;
        private readonly IssueService issueService;

        public IssueController(
            ILogger<IssueController> logger,
            IssueService issueService
        )
        {
            this.logger = logger;
            this.issueService = issueService;

            logger.LogDebug(1, "construct IssueController");
        }

        /// <summary>
        /// Получить список выдачи книг
        /// </summary>
        [HttpGet]
        public async Task<List<Issue>> IssueRead()
        {
            List<Issue> issues = await issueService.getIssue();

            return issues;
        }

        /// <summary>
        /// Получить выдачу книги по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> IssueRead(int id)
        {
            Issue issue = await issueService.getIssue(id);
            return Ok(issue);
        }

        /// <summary>
        /// Добавить выдачу книги
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> IssueCreate(IssueRequest issue)
        {
            Issue newIssue = new Issue
            {
                StatusId = issue.StatusId,
                EmployeeId = issue.EmployeeId,
                ReaderId = issue.ReaderId,
                InstanceId = issue.InstanceId,
                DateOfIssue = issue.DateOfIssue,
                ReturnDate = issue.ReturnDate
            };

            int newId = await issueService.addIssue(newIssue);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить выдачу книги
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> IssueUpdate(IssueRequestForUpdate issue)
        {
            await issueService.updateIssue(issue);
            return Ok();
        }

        /// <summary>
        /// Удалить выдачу книги
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> IssueDelete(int? id)
        {
            await issueService.removeIssue(id);
            return Ok();
        }
    }
}
