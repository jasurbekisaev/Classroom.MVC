using System.ComponentModel.DataAnnotations;

namespace Classroom.MVC.Models;

public class UpdateUserDto
{
    [Required]
    public string UserName { get; set; }

    public string FirstName { get; set; }

    public IFormFile? UserPhoto { get; set; }
}
