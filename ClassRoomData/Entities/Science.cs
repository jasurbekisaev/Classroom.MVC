using System.ComponentModel.DataAnnotations;

namespace ClassRoomData.Entities
{
    public class Science
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid SchoolId { get; set; }
        public School School { get; set; }
        public List<UserScience>? UserSciences { get; set; }
        public List<TaskEntity> Tasks { get; set; }
    }

}
