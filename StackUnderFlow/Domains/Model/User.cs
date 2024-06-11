using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Domains.Model;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Script> Programs { get; set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Sharing> Sharings { get; set; }
    public ICollection<ScriptVersion> Versions { get; set; }
    public ICollection<Pipeline> Pipelines { get; set; }
    public ICollection<FriendRequest> Friends { get; set; }
    public ICollection<Follow> Follower { get; set; }
    public ICollection<Follow> Followed { get; set; }
    public ICollection<GroupRequest> GroupRequests { get; set; }
}
