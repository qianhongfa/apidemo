using Microsoft.EntityFrameworkCore;
using MiniApp.Models;

namespace MiniApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<ProductModule> ProductModules => Set<ProductModule>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantProductModule> TenantProductModules => Set<TenantProductModule>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<OperationLog> OperationLogs => Set<OperationLog>();
}
