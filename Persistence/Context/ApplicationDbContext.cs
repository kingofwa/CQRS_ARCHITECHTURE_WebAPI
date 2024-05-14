using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Rate).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // Seed Data
            var hasher = new PasswordHasher<User>();

            // Users
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = hasher.HashPassword(null, "admin123")
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                Username = "client",
                PasswordHash = hasher.HashPassword(null, "client123")
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 3,
                Username = "office",
                PasswordHash = hasher.HashPassword(null, "office123")
            });

            // Roles
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin"
            });

            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 2,
                Name = "Client"
            });

            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 3,
                Name = "Office"
            });

            // Permissions
            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                Id = 1,
                Name = "Edit"
            });

            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                Id = 2,
                Name = "Create"
            });

            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                Id = 3,
                Name = "Get"
            });

            // UserRoles
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = 1,
                RoleId = 1
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = 2,
                RoleId = 2
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = 3,
                RoleId = 3
            });

            // RolePermissions
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 1,
                PermissionId = 1
            });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 1,
                PermissionId = 2
            });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 1,
                PermissionId = 3
            });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 2,
                PermissionId = 3
            });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 3,
                PermissionId = 2
            });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                RoleId = 3,
                PermissionId = 3
            });

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Category> Category { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}

