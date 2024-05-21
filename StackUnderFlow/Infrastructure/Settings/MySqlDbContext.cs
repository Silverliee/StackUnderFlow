using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using Version = System.Version;

namespace StackUnderFlow.Infrastructure.Settings;

public class MySqlDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Script> Scripts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Sharing> Sharings { get; set; }
    public DbSet<Version> Versions { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseMySql("YourConnectionString", ServerVersion.AutoDetect("YourConnectionString"));
    }
}