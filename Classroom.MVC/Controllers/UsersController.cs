using Classroom.MVC.Helpers;
using Classroom.MVC.Models;
using ClassRoomData.Context;
using ClassRoomData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.MVC.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<User> _usermanager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _context;

    public UsersController(UserManager<User> usermanager, SignInManager<User> signInManager, AppDbContext context)
    {
        _usermanager = usermanager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] CreateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User()
        {
            FirstName = model.FirstName,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Age = model.Age,
        };

        user.PhotoUrl = await FileHelper.SaveUserFile(model.Photo);

        var result = await _usermanager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Username", result.Errors.First().Description);

            return View();
        }
        await _signInManager.SignInAsync(user, isPersistent: true);

        return RedirectToAction("Profile");
    }


    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _usermanager.GetUserAsync(User);
        return View(user);
    }


    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> SignIn([FromForm] SigninUserDto signinUserDto)
    {
        var result = await _signInManager.PasswordSignInAsync(signinUserDto.UserName, signinUserDto.Password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("UserName", " UserName or Password is not found");
            return View();
        }


        return RedirectToAction("Profile");
    }


    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    ///    update user
    [HttpGet]
    public async Task<IActionResult> UpdateUser(Guid id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return RedirectToAction("SignUp");
        }

        ViewBag.Id = id;

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return RedirectToAction("SignUp");
        }

        user.FirstName = updateUserDto.FirstName;
        user.UserName = updateUserDto.UserName;

        if (updateUserDto.UserPhoto != null)
        {
            user.PhotoUrl = await FileHelper.SaveSchoolFile(updateUserDto.UserPhoto);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Profile");
    }

}
