using MiniApp.Models;

namespace MiniApp.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Menus.Any())
        {
            db.Menus.AddRange(new Menu { Name = "Home", Url = "/" }, new Menu { Name = "Products", Url = "/products" });
        }
        if (!db.ProductModules.Any())
        {
            db.ProductModules.Add(new ProductModule { Name = "Base Product" });
        }
        if (!db.Permissions.Any()) db.Permissions.Add(new Permission { Name = "Edit" });
        if (!db.Roles.Any()) db.Roles.Add(new Role { Name = "Admin" });
        if (!db.Tenants.Any()) db.Tenants.Add(new Tenant { Name = "Default Tenant" });
        if (!db.Users.Any()) db.Users.Add(new User { UserName = "admin", DisplayName = "Administrator" });
        db.SaveChanges();
    }
}
