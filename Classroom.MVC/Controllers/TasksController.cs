using Classroom.MVC.Helpers;
using Classroom.MVC.Models;
using ClassRoomData.Context;
using ClassRoomData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.MVC.Controllers;

public class TasksController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserProvider _userProvider;

    public TasksController(AppDbContext context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }

    public async Task<IActionResult> Index(Guid scienceId)
    {
        var tasks = await _context.TaskEntities
            .Where(t => t.ScienceId == scienceId)
            .ToListAsync();

        ViewBag.ScienceId = scienceId;
        return View(tasks);
    }

    [HttpGet]
    public IActionResult CreateScienceTask(Guid scienceId)
    {
        ViewBag.ScienceId = scienceId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateScienceTask([FromForm] CreateTaskDto createTaskDto, Guid scienceId)
    {
        var task = new TaskEntity()
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            StartDate = createTaskDto.StartDate,
            EndDate = createTaskDto.EndDate,
            Status = ETaskStatus.Active,
            MaxBall = createTaskDto.MaxBall,
            ScienceId = scienceId
        };

        _context.TaskEntities.Add(task);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", new { scienceId });
    }

    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
        var task = await _context.TaskEntities
            .Include(t => t.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t => t.Id == taskId);


        return View(task);
    }

    public async Task<IActionResult> AddComment(Guid taskId, [FromForm] CreateCommentDto commentDto)
    {
        var comment = new TaskComment()
        {
            Comment = commentDto.Comment,
            TaskId = taskId,
            UserId = _userProvider.UserId,
        };

        _context.TaskComments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetTaskById", new { taskId });
    }
}

