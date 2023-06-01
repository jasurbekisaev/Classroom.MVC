using System.ComponentModel.DataAnnotations;

namespace Classroom.MVC.Models;

public class CreateUserDto
{
    public string FirstName { get; set; }
    public string UserName { get; set; }

    [Phone]
    [StringLength(9)]
    public string PhoneNumber { get; set; }

    public int Age { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [MaxLength(16)]
    [MinLength(8)]
    public string Password { get; set; }
    public IFormFile Photo { get; set; }
}
