
namespace ClassRoomData.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ETaskStatus Status { get; set; }
    public ushort MaxBall { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Guid ScienceId { get; set; }
    public Science Science { get; set; }

    public List<UserTask> UserTasks { get; set; }

    public List<TaskComment> Comments { get; set; }
}

