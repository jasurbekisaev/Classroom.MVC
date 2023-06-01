using Classroom.MVC.Helpers;
using Classroom.MVC.Models;
using ClassRoomData.Context;
using ClassRoomData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.MVC.Controllers;

public class SciencesController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserProvider _userProvider;

    public SciencesController(AppDbContext context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }

    public async Task<IActionResult> Index(Guid schoolId, string? name = null, bool orderByUsers = false)
    {
        /*var school = await _context.Schools
            .Include(s => s.Sciences)
            .ThenInclude(s => s.UserSciences)
            .FirstOrDefaultAsync(s => s.Id == schoolId);

        return View(school);*/
        var query = _context.Sciences
            .Include(s => s.UserSciences);


        ViewBag.SchoolId = schoolId;
        if (name == null)
        {
            var school = await _context.Schools
                .Include(s => s.Sciences)
                .ThenInclude(s => s.UserSciences)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            school = await _context.Schools
                .Include(s => s.UserSchools)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            if (school == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(school);
        }
        else
        {
            var school = await _context.Schools
                .Include(s => s.Sciences.Where(s => s.Name.Contains(name)))
                .ThenInclude(s => s.UserSciences)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            school = await _context.Schools
                .Include(s => s.UserSchools)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            return View(school);
        }
    }

    public async Task<IActionResult> GetScienceById(Guid scienceId)
    {
        var science = await _context.Sciences
            .Include(s => s.School)
            .Include(s => s.UserSciences)
            .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == scienceId);

        return View(science);
    }


    [HttpGet]
    public IActionResult CreateScience(Guid schoolId)
    {
        ViewBag.SchoolId = schoolId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateScience(Guid schoolId, [FromForm] CreateScienceDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createDto);
        }
        var school = new Science()
        {
            Name = createDto.Name,
            Description = createDto.Description,
            SchoolId = schoolId
        };

        school.UserSciences = new List<UserScience>
        {
            new UserScience()
            {
                UserId = _userProvider.UserId,
                Type = EUserScience.Teacher
            }
        };

        _context.Sciences.Add(school);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", new { schoolId = schoolId });
    }

    [HttpGet]
    public IActionResult SendJoinScienceRequest(Guid scienceId)
    {
        ViewBag.ScienceId = scienceId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendJoinScienceRequest(Guid scienceId,
        [FromForm] CreateJoinScienceRequestDto joinRequestDto)
    {
        var toUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == joinRequestDto.UserName);

        if (toUser == null)
        {
            return RedirectToAction("Index", "Schools");
        }
        var isExistsPreviewJoinRequest =
        await _context.JoinScienceRequests
                .AnyAsync(r => r.ToUserId == toUser.Id && r.ScienceId == scienceId);

        if (isExistsPreviewJoinRequest)
        {
            return RedirectToAction("GetScienceById", new { scienceId });
        }

        var isExistsInScience =
            await _context.UserSciences
                .AnyAsync(u => u.UserId == toUser.Id && u.ScienceId == scienceId);
        if (isExistsInScience)
        {
            return RedirectToAction("GetScienceById", new { scienceId });
        }

        var userId = _userProvider.UserId;

        var joinRequest = new JoinScienceRequest()
        {
            FromUserId = userId,
            ToUserId = toUser.Id,
            ScienceId = scienceId,
            IsJoined = false,

        };

        _context.JoinScienceRequests.Add(joinRequest);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetScienceById", new { scienceId });
    }

    public async Task<IActionResult> JoinScience(bool isJoin, Guid joinRequestId)
    {
        var joinRequest = await _context.JoinScienceRequests.FirstOrDefaultAsync(u => u.Id == joinRequestId && u.ToUserId == _userProvider.UserId);
        if (isJoin)
        {
            var userScience = new UserScience()
            {
                ScienceId = joinRequest.ScienceId,
                UserId = joinRequest.ToUserId,
                Type = EUserScience.Student,
            };
            joinRequest.IsJoined = true;
            _context.UserSciences.Add(userScience);
        }
        else
        {
            _context.JoinScienceRequests.Remove(joinRequest);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Profile", "Users");
    }

    // Remove funksiyasi yozilgan joy , get science by id da ham if li qismida buni front yozilgan
    public async Task<IActionResult> Remove(Guid scienceId, Guid Uid, Guid schoolId)
    {

        var s = await _context.UserSciences.FirstAsync(s => s.UserId == Uid && s.ScienceId == scienceId);

        _context.UserSciences.Remove(s);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetScienceById", new { scienceId, schoolId });
    }


}
