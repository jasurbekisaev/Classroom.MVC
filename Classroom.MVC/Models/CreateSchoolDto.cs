using System.ComponentModel.DataAnnotations;

namespace Classroom.MVC.Models;

public class CreateSchoolDto
{
    [Required]
    [MaxLength(50)]
    [MinLength(5)]
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
    //public DateTime? CreatedTime { get; set; }
    public IFormFile? LogoPhoto { get; set; }

}
