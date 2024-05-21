namespace StackUnderFlow.Domains.Model;

public class User
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Script> Programs { get; set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Sharing> Sharings { get; set; }
    public ICollection<Version> Versions { get; set; }
    public ICollection<Pipeline> Pipelines { get; set; }
}