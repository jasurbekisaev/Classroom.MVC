
using Microsoft.AspNetCore.Identity;

namespace ClassRoomData.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public int Age { get; set; }
    public List<UserSchool> UserSchools { get; set; }
    public List<UserScience>? UserSciences { get; set; }
    public List<UserTask> UserTasks { get; set; }
    public List<TaskComment> TaskComments { get; set; }
}
