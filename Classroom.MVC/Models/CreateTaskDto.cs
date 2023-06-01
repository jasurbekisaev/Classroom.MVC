namespace Classroom.MVC.Models;

public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ushort MaxBall { get; set; }
}

