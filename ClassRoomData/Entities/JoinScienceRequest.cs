
namespace ClassRoomData.Entities;

public class JoinScienceRequest
{
    public Guid Id { get; set; }
    public Guid ScienceId { get; set; }
    public Science Science { get; set; }
    public Guid ToUserId { get; set; }
    public Guid FromUserId { get; set; }
    public User FromUser { get; set; }
    public bool IsJoined { get; set; }
}

