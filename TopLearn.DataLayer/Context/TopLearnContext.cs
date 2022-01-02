using Microsoft.EntityFrameworkCore;
using System.Linq;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {
       public TopLearnContext(DbContextOptions<TopLearnContext> options) : base(options)
        {

        }
        #region User
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion

        #region Wallet 
        public DbSet<WalletType> Wallettypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region Permission
        public DbSet<Permission> permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        #endregion

        #region Course
        public DbSet<CourseGroup> CourseGroup { get; set; }
        public DbSet<CourseLevel> CourseLevel { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseEpisode> CourseEpisode { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        #endregion

        #region Order
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderDetail> OrderDetails { get; set;}
        public DbSet<Discount> Discounts { get; set;}

    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete);
            modelBuilder.Entity<CourseGroup>()
                .HasQueryFilter(g => !g.IsDelete);
            base.OnModelCreating(modelBuilder);
        }
    }
}
