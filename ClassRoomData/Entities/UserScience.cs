﻿
namespace ClassRoomData.Entities
{
    public class UserScience
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ScienceId { get; set; }
        public Science Science { get; set; }
        public EUserScience Type { get; set; }

    }
}
