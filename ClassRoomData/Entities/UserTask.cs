
namespace ClassRoomData.Entities;

public class UserTask
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid TaskId { get; set; }
    public TaskEntity Task { get; set; }

    public ushort Ball { get; set; }
    public EUserTaskStatus Status { get; set; }
    public string Comment { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}

