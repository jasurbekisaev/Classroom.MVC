using Classroom.MVC.Helpers;
using Classroom.MVC.Models;
using ClassRoomData.Context;
using ClassRoomData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.MVC.Controllers;

public class SchoolsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserProvider _userProvider;

    public SchoolsController(AppDbContext context, UserProvider userProvider)
    {
        _context = context;
        _userProvider = userProvider;
    }

    [HttpGet]
    public IActionResult CreateSchool()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchool([FromForm] CreateSchoolDto createschool)
    {
        /*
         if (!ModelState.IsValid)
        {
            return View(createschool);
        }
        */

        var school = new School()
        {
            Name = createschool.Name,
            Description = createschool.Description,
        };

        if (createschool.LogoPhoto != null)
        {
            school.LogoUrl = await FileHelper.SaveSchoolFile(createschool.LogoPhoto);
        }
        school.UserSchools = new List<UserSchool>()
        {
            new UserSchool()
            {
                UserId = _userProvider.UserId,
                Type = EUserSchool.Founder,
            }
        };

        _context.Schools.Add(school);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Index()
    {
        var schools = await _context.Schools
            .Include(school => school.UserSchools)
            .ToListAsync();
        return View(schools);
    }


    public async Task<IActionResult> GetSchoolById(Guid id)
    {
        var userId = _userProvider.UserId;
        if (userId == null)
        {
            return RedirectToAction("SignUp", "Users");
        }
        var school = await _context.Schools
            .Include(school => school.UserSchools)
                .ThenInclude(userschool => userschool.User)
                    .FirstOrDefaultAsync(x => x.Id == id);

        return View(school);
    }

    public async Task<IActionResult> JoinSchool(Guid id)
    {
        var school = await _context.Schools
                     .Include(school => school.UserSchools)
                        .ThenInclude(u => u.User)
                            .FirstOrDefaultAsync(userschool => userschool.Id == id);

        var userId = _userProvider.UserId;
        if (!school.UserSchools.Any(u => u.User.Id == userId))
        {
            school.UserSchools.Add(new UserSchool()
            {
                UserId = userId,
                Type = EUserSchool.student,
            });
        }

        /*var userschool = new UserSchool()
        {
            UserId = userId,
            SchoolId = id,
            Type = EUserSchool.student
        };*/
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSchoolById", new { id = school.Id });
    }


    [HttpGet]
    public async Task<IActionResult> UpdateSchool(Guid id)
    {
        var school = await _context.Schools
            .Include(school => school.UserSchools)
            .ThenInclude(userschool => userschool.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        ViewBag.Id = id;
        return View(new UpdateSchoolDto()
        {
            Name = school.Name,
            Description = school.Description,
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSchool(Guid id, [FromForm] UpdateSchoolDto updateschool)
    {
        var school = await _context.Schools.FirstOrDefaultAsync(u => u.Id == id);

        school.Name = updateschool.Name;
        school.Description = updateschool.Description;
        if (updateschool.LogoPhoto != null)
        {
            school.LogoUrl = await FileHelper.SaveSchoolFile(updateschool.LogoPhoto);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("GetSchoolById", new { id = school.Id });
    }


    public async Task<IActionResult> UpdateUserRole(Guid schoolId, Guid userId, EUserSchool role)
    {
        var userSchool = await _context.UserSchools
            .FirstOrDefaultAsync(u => u.UserId == userId && u.SchoolId == schoolId);

        if (userSchool.Type != EUserSchool.Founder && role != EUserSchool.Founder)
            userSchool.Type = role;

        await _context.SaveChangesAsync();

        return RedirectToAction("GetSchoolById", new { id = schoolId });
    }
}
