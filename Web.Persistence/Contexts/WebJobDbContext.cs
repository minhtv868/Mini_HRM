using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Common.Interfaces;
using Web.Domain.Entities.Identity;
using Web.Domain.Entities.Jobs;
using Web.Persistence.Configurations.Identity;

namespace Web.Persistence.Contexts
{
    public class WebJobDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public WebJobDbContext(DbContextOptions<WebJobDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<JobQueue> JobQueues { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Bỏ tiền tố AspNet
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                    entityType.SetTableName(tableName.Substring(6));
            }

            // Áp dụng các cấu hình trong assembly có IEntityTypeConfigurationBongDa24hJobDbContext<>
            builder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfigurationBongDa24hJobDbContext<>))
            );

            // Cấu hình Identity
            new RoleConfig().Configure(builder.Entity<Role>());
            new SysFunctionConfig().Configure(builder.Entity<SysFunction>());
            new SysFunctionRoleConfig().Configure(builder.Entity<SysFunctionRole>());
            new SysFunctionUserConfig().Configure(builder.Entity<SysFunctionUser>());
            new SysLogConfig().Configure(builder.Entity<SysLog>());
            new UserConfig().Configure(builder.Entity<User>());
            new UserLogConfig().Configure(builder.Entity<UserLog>());

            // Seed dữ liệu
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
            SeedSysFunctions(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            User user = new User()
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@gmail.com",
                FullName = "Administrator",
                NormalizedUserName = "admin",
                NormalizedEmail = "admin@gmail.com",
                SecurityStamp = "",
                ConcurrencyStamp = ""
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "@minhtv123");

            builder.Entity<User>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
              new Role { Id = 1, Name = "Super Admin", NormalizedName = "SUPER ADMIN", Description = "Super Admin", ConcurrencyStamp = "1" },
              new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN", Description = "Administrator", ConcurrencyStamp = "1" },
              new Role { Id = 3, Name = "Quảng cáo", NormalizedName = "ADVERT", Description = "Quảng cáo", ConcurrencyStamp = "2" },
              new Role { Id = 4, Name = "Customer", NormalizedName = "CUSTOMER", Description = "Khách hàng", ConcurrencyStamp = "2" }
            );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { RoleId = 1, UserId = 1 },
                new IdentityUserRole<int> { RoleId = 2, UserId = 1 }
            );
        }

        private void SeedSysFunctions(ModelBuilder builder)
        {
            builder.Entity<SysFunction>().HasData(
                new SysFunction { Id = 1, FunctionName = "Hệ thống", FunctionDesc = "Hệ thống", Url = "/", IconPath = "nav-icon fas fa-cog", IsShow = true, DisplayOrder = 10, TreeLevel = 1, TreeOrder = 1 },
                new SysFunction { Id = 2, FunctionName = "Danh sách user", FunctionDesc = "Danh sách user", Url = "/Identity/SysUsers", ParentItemId = 1, DisplayOrder = 1, TreeLevel = 2, TreeOrder = 2 },
                new SysFunction { Id = 3, FunctionName = "Danh sách quyền", FunctionDesc = "Danh sách quyền", Url = "/Identity/SysRoles", ParentItemId = 1, DisplayOrder = 2, TreeLevel = 2, TreeOrder = 3 },
                new SysFunction { Id = 4, FunctionName = "Danh sách chức năng", FunctionDesc = "Danh sách chức năng", Url = "/Identity/SysFunctions", ParentItemId = 1, DisplayOrder = 3, TreeLevel = 2, TreeOrder = 4 },
                new SysFunction { Id = 5, FunctionName = "Log đăng nhập", FunctionDesc = "Log đăng nhập", Url = "/Identity/UserLogs", ParentItemId = 1, DisplayOrder = 4, TreeLevel = 2, TreeOrder = 5 },
                new SysFunction { Id = 6, FunctionName = "Log hệ thống", FunctionDesc = "Log hệ thống", Url = "/Identity/SysLogs", ParentItemId = 1, DisplayOrder = 5, TreeLevel = 2, TreeOrder = 6 },
                new SysFunction { Id = 7, FunctionName = "Log file", FunctionDesc = "Log file", Url = "/Identity/SysLogFiles", ParentItemId = 1, DisplayOrder = 6, TreeLevel = 2, TreeOrder = 7 }
            );

            builder.Entity<SysFunctionRole>().HasData(
                new SysFunctionRole { SysFunctionId = 1, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 2, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 3, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 4, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 5, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 6, RoleId = 1 },
                new SysFunctionRole { SysFunctionId = 7, RoleId = 1 }
            );
        }
    }
}
