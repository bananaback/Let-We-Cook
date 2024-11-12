
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.Property(u => u.IsRemoved)
                .HasColumnName("is_removed");

            builder.Property(u => u.Balance)
                .HasColumnName("balance")
                .HasColumnType("decimal(18,2)");


            builder.HasMany(u => u.ShoppingLists)
                .WithOne(sl => sl.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.Recipes)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasMany(u => u.MealPlans)
                .WithOne(ml => ml.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.DishCollection)
                .WithOne(dc => dc.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.Feeds)
                .WithOne(f => f.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasMany(u => u.Activities)
                .WithOne(a => a.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.RecipeReviews)
                .WithOne(rr => rr.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.Followings)
                .WithOne(f => f.Follower)
                .HasForeignKey("FollowerId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(u => u.Followers)
                .WithOne(f => f.Followed)
                .HasForeignKey("FollowedId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(u => u.Badges)
                .WithMany(b => b.Users)
                .UsingEntity<UserBadge>(
                    j => j.ToTable("user_badge")
                        .HasOne(ub => ub.Badge)
                        .WithMany(b => b.UserBadges)
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j => j.HasOne(ub => ub.User)
                        .WithMany(u => u.UserBadges)
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j =>
                    {
                        j.Property<Guid>("ApplicationUserId")
                            .HasColumnName("user_id");

                        j.Property<Guid>("BadgeId")
                            .HasColumnName("badge_id");

                        j.HasKey("ApplicationUserId", "BadgeId");

                        j.Property(j => j.DateEarned)
                            .HasColumnName("date_earned");
                    }
                );

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.TaughtCourses)
                .WithOne(c => c.Instructor)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.EnrolledCourses)
                .WithMany(c => c.EnrolledStudents)
                .UsingEntity<Enrollment>(
                    j => j.ToTable("enrollment")
                        .HasOne(e => e.Course)
                        .WithMany(c => c.Enrollments)
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired(),
                    j => j.HasOne(e => e.User)
                        .WithMany(u => u.Enrollments)
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j =>
                    {
                        j.Property<Guid>("ApplicationUserId")
                            .HasColumnName("user_id");

                        j.Property<Guid>("CourseId")
                            .HasColumnName("course_id");

                        j.HasKey("ApplicationUserId", "CourseId");

                        j.Property(j => j.DateEnrolled)
                            .HasColumnName("date_enrolled");

                        j.Property(j => j.Progress)
                            .HasColumnName("progress")
                            .HasPrecision(18, 2);
                    }
                );

            builder.HasMany(u => u.QuizResults)
                .WithOne(qr => qr.User)
                .HasForeignKey("ApplicationUserId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
