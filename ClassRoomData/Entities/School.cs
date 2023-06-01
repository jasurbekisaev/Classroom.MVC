using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClassRoomData.Entities;

[Table("schools")]
public class School
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [Column("name")]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public List<UserSchool> UserSchools { get; set; }
    public List<Science> Sciences { get; set; }

}
