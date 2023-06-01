
using ClassRoomData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassRoomData.Context.Configurations;

public class UserSchoolConfiguration : IEntityTypeConfiguration<UserSchool>
{
    public void Configure(EntityTypeBuilder<UserSchool> builder)
    {
        builder.ToTable("user_school")
            .HasKey(u => new { u.UserId, u.SchoolId });

        builder.HasOne(u => u.User)
            .WithMany(u => u.UserSchools)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(a => a.School)
            .WithMany(a => a.UserSchools)
            .HasForeignKey(a => a.SchoolId);

    }
}
