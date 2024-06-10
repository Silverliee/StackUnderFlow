using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Infrastructure.Settings;

public class MySqlDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MySqlDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User?> Users { get; set; }
    public DbSet<Script?> Scripts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Sharing> Sharings { get; set; }
    public DbSet<ScriptVersion> ScriptVersions { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<Status> Statuses { get; set; }
    
    public DbSet<FriendRequest> Friends { get; set; }
    public DbSet<Follow> Follows { get; set; }
    
    public DbSet<GroupRequest> GroupRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("database"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration des relations et des clés primaires
        modelBuilder.Entity<Comment>()
            .HasKey(c => c.CommentId);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<FriendRequest>()
            .HasKey(f => new { f.UserId1, f.UserId2 });
        modelBuilder.Entity<FriendRequest>()
            .HasOne(f => f.User1)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserId1)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<GroupRequest>()
            .HasKey(gr => new { gr.GroupId, gr.UserId });

        modelBuilder.Entity<GroupRequest>()
            .HasOne(gr => gr.User)
            .WithMany(u => u.GroupRequests)
            .HasForeignKey(gr => gr.UserId);

        modelBuilder.Entity<GroupRequest>()
            .HasOne(gr => gr.Group)
            .WithMany(g => g.GroupRequests)
            .HasForeignKey(gr => gr.GroupId);

        modelBuilder.Entity<Follow>()
            .HasKey(f => new { f.UserId1, f.UserId2 });
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.User1)
            .WithMany(u => u.Follower)
            .HasForeignKey(f => f.UserId1)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.User2)
            .WithMany(u => u.Followed)
            .HasForeignKey(f => f.UserId2)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Group>()
            .HasKey(g => g.GroupId);

        modelBuilder.Entity<Like>()
            .HasKey(l => l.LikeId);
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.NoAction) ;

        modelBuilder.Entity<Pipeline>()
            .HasKey(p => p.PipelineId);
        modelBuilder.Entity<Pipeline>()
            .HasOne(p => p.Creator)
            .WithMany(u => u.Pipelines)
            .HasForeignKey(p => p.CreatorUserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<Pipeline>()
            .HasOne(p => p.Status)
            .WithMany()
            .HasForeignKey(p => p.StatusId);

        modelBuilder.Entity<Script>()
            .HasKey(s => s.ScriptId);
        modelBuilder.Entity<Script>()
            .HasOne(s => s.User)
            .WithMany(u => u.Programs)
            .HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<Script>()
            .HasMany(s => s.Sharings)
            .WithOne(sh => sh.Script)
            .HasForeignKey(sh => sh.ScritpId);
        modelBuilder.Entity<Script>()
            .HasMany(s => s.Versions)
            .WithOne(v => v.Script)
            .HasForeignKey(v => v.ScriptId);

        modelBuilder.Entity<Sharing>()
            .HasKey(sh => new { sh.ScritpId, sh.UserId });
        modelBuilder.Entity<Sharing>()
            .HasOne(sh => sh.Script)
            .WithMany(s => s.Sharings)
            .HasForeignKey(sh => sh.ScritpId);
        modelBuilder.Entity<Sharing>()
            .HasOne(sh => sh.User)
            .WithMany(u => u.Sharings)
            .HasForeignKey(sh => sh.UserId).OnDelete(DeleteBehavior.NoAction) ;

        modelBuilder.Entity<Status>()
            .HasKey(st => st.StatusId);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Programs)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<User>()
            .HasMany(u => u.Sharings)
            .WithOne(sh => sh.User)
            .HasForeignKey(sh => sh.UserId).OnDelete(DeleteBehavior.NoAction) ;
        modelBuilder.Entity<User>()
            .HasMany(u => u.Versions)
            .WithOne(v => v.Creator)
            .HasForeignKey(v => v.CreatorUserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Pipelines)
            .WithOne(p => p.Creator)
            .HasForeignKey(p => p.CreatorUserId);

        modelBuilder.Entity<ScriptVersion>()
            .HasKey(v => v.ScriptVersionId);

        modelBuilder.Entity<ScriptVersion>()
            .HasOne(v => v.Script)
            .WithMany(s => s.Versions)
            .HasForeignKey(v => v.ScriptId);
        
        modelBuilder.Entity<ScriptVersion>()
            .HasOne(v => v.Creator)
            .WithMany(s => s.Versions)
            .HasForeignKey(v => v.CreatorUserId);

        // Configuration des longueurs de chaînes et autres contraintes
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(200)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .IsRequired();

        modelBuilder.Entity<Group>()
            .Property(g => g.GroupName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Script>()
            .Property(s => s.ScriptName)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Script>()
            .Property(s => s.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<Status>().HasData(
            new Status { StatusId = 1, Label = "Pending" },
            new Status { StatusId = 2, Label = "Completed" }
        );

        base.OnModelCreating(modelBuilder);
    }
}