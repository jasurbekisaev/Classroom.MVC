using ClassRoomData.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassRoomData.Context;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<School> Schools { get; set; }
    public DbSet<UserSchool> UserSchools { get; set; }
    public DbSet<Science> Sciences { get; set; }
    public DbSet<UserScience> UserSciences { get; set; }
    public DbSet<JoinScienceRequest> JoinScienceRequests { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskEntity> TaskEntities { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //new UserSchoolConfiguration().Configure(builder.Entity<UserSchool>());
        //builder.ApplyConfiguration(new UserSchoolConfiguration());
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        builder.Entity<User>()
            .ToTable("users");

        builder.Entity<User>()
            .Property(x => x.FirstName)
            .IsRequired(true)
            .HasMaxLength(50)
            .HasDefaultValue("Hello World")
            .HasColumnName("firstname");

        builder.Entity<User>()
            .Property(x => x.LastName)
            .HasMaxLength(50)
            .HasColumnName("lastname");

        builder.Entity<User>()
            .Property(x => x.PhotoUrl)
            .IsRequired(false)
            .HasColumnName("photourl");

        builder.Entity<UserTask>().HasKey(t => new { t.UserId, t.TaskId });



    }
}


