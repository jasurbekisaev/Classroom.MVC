
using ClassRoomData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassRoomData.Context.Configurations
{
    public class UserScienceConfiguration : IEntityTypeConfiguration<UserScience>
    {
        public void Configure(EntityTypeBuilder<UserScience> builder)
        {
            builder.HasKey(s => new { s.UserId, s.ScienceId });

            /*builder.HasOne(u => u.User)
                .WithMany(u => u.UserSciences)
                .HasForeignKey(u => u.UserId);

            builder.HasOne(a => a.Science)
                .WithMany(a => a.UserSciences)
                .HasForeignKey(a => a.ScienceId);*/

        }
    }
}
