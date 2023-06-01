namespace Classroom.MVC.Models
{
    public class UpdateSchoolDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        //public DateTime? CreatedTime { get; set; }
        public IFormFile? LogoPhoto { get; set; }

    }
}
