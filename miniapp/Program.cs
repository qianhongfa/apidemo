using Microsoft.EntityFrameworkCore;
using MiniApp.Data;
using MiniApp.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("MiniAppDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

// Generic endpoints per entity
app.MapGet("/api/{entity}", async (string entity, AppDbContext db) =>
{
    return entity.ToLower() switch
    {
        "menus" => Results.Ok(await db.Menus.ToListAsync()),
        "product_modules" => Results.Ok(await db.ProductModules.ToListAsync()),
        "permissions" => Results.Ok(await db.Permissions.ToListAsync()),
        "roles" => Results.Ok(await db.Roles.ToListAsync()),
        "role_permissions" => Results.Ok(await db.RolePermissions.ToListAsync()),
        "tenants" => Results.Ok(await db.Tenants.ToListAsync()),
        "tenants_product_modules" => Results.Ok(await db.TenantProductModules.ToListAsync()),
        "users" => Results.Ok(await db.Users.ToListAsync()),
        "user_roles" => Results.Ok(await db.UserRoles.ToListAsync()),
        "usergroups" => Results.Ok(await db.UserGroups.ToListAsync()),
        "organizations" => Results.Ok(await db.Organizations.ToListAsync()),
        "operation_logs" => Results.Ok(await db.OperationLogs.ToListAsync()),
        _ => Results.NotFound()
    };
});

app.MapGet("/api/{entity}/{id}", async (string entity, int id, AppDbContext db) =>
{
    object? item = entity.ToLower() switch
    {
        "menus" => await db.Menus.FindAsync(id),
        "product_modules" => await db.ProductModules.FindAsync(id),
        "permissions" => await db.Permissions.FindAsync(id),
        "roles" => await db.Roles.FindAsync(id),
        "role_permissions" => await db.RolePermissions.FindAsync(id),
        "tenants" => await db.Tenants.FindAsync(id),
        "tenants_product_modules" => await db.TenantProductModules.FindAsync(id),
        "users" => await db.Users.FindAsync(id),
        "user_roles" => await db.UserRoles.FindAsync(id),
        "usergroups" => await db.UserGroups.FindAsync(id),
        "organizations" => await db.Organizations.FindAsync(id),
        "operation_logs" => await db.OperationLogs.FindAsync(id),
        _ => null
    };
    return item is null ? Results.NotFound() : Results.Ok(item);
});

app.MapPost("/api/{entity}", async (string entity, HttpRequest req, AppDbContext db) =>
{
    try
    {
        switch (entity.ToLower())
        {
            case "menus":
                var menu = await req.ReadFromJsonAsync<Menu>(); db.Menus.Add(menu!); await db.SaveChangesAsync(); return Results.Created($"/api/menus/{menu!.Id}", menu);
            case "product_modules":
                var pm = await req.ReadFromJsonAsync<ProductModule>(); db.ProductModules.Add(pm!); await db.SaveChangesAsync(); return Results.Created($"/api/product_modules/{pm!.Id}", pm);
            case "permissions":
                var p = await req.ReadFromJsonAsync<Permission>(); db.Permissions.Add(p!); await db.SaveChangesAsync(); return Results.Created($"/api/permissions/{p!.Id}", p);
            case "roles":
                var r = await req.ReadFromJsonAsync<Role>(); db.Roles.Add(r!); await db.SaveChangesAsync(); return Results.Created($"/api/roles/{r!.Id}", r);
            case "role_permissions":
                var rp = await req.ReadFromJsonAsync<RolePermission>(); db.RolePermissions.Add(rp!); await db.SaveChangesAsync(); return Results.Created($"/api/role_permissions/{rp!.Id}", rp);
            case "tenants":
                var t = await req.ReadFromJsonAsync<Tenant>(); db.Tenants.Add(t!); await db.SaveChangesAsync(); return Results.Created($"/api/tenants/{t!.Id}", t);
            case "tenants_product_modules":
                var tpm = await req.ReadFromJsonAsync<TenantProductModule>(); db.TenantProductModules.Add(tpm!); await db.SaveChangesAsync(); return Results.Created($"/api/tenants_product_modules/{tpm!.Id}", tpm);
            case "users":
                var u = await req.ReadFromJsonAsync<User>(); db.Users.Add(u!); await db.SaveChangesAsync(); return Results.Created($"/api/users/{u!.Id}", u);
            case "user_roles":
                var ur = await req.ReadFromJsonAsync<UserRole>(); db.UserRoles.Add(ur!); await db.SaveChangesAsync(); return Results.Created($"/api/user_roles/{ur!.Id}", ur);
            case "usergroups":
                var ug = await req.ReadFromJsonAsync<UserGroup>(); db.UserGroups.Add(ug!); await db.SaveChangesAsync(); return Results.Created($"/api/usergroups/{ug!.Id}", ug);
            case "organizations":
                var o = await req.ReadFromJsonAsync<Organization>(); db.Organizations.Add(o!); await db.SaveChangesAsync(); return Results.Created($"/api/organizations/{o!.Id}", o);
            case "operation_logs":
                var l = await req.ReadFromJsonAsync<OperationLog>(); db.OperationLogs.Add(l!); await db.SaveChangesAsync(); return Results.Created($"/api/operation_logs/{l!.Id}", l);
            default: return Results.NotFound();
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPut("/api/{entity}/{id}", async (string entity, int id, HttpRequest req, AppDbContext db) =>
{
    try
    {
        switch (entity.ToLower())
        {
            case "menus":
                var menu = await req.ReadFromJsonAsync<Menu>();
                var existing = await db.Menus.FindAsync(id);
                if (existing is null) return Results.NotFound();
                existing.Name = menu!.Name; existing.Url = menu.Url; await db.SaveChangesAsync(); return Results.Ok(existing);
            // For brevity, apply similar simple field updates for others
            default:
                return Results.NotFound();
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapDelete("/api/{entity}/{id}", async (string entity, int id, AppDbContext db) =>
{
    switch (entity.ToLower())
    {
        case "menus":
            var m = await db.Menus.FindAsync(id); if (m is null) return Results.NotFound(); db.Menus.Remove(m); await db.SaveChangesAsync(); return Results.Ok();
        case "product_modules":
            var pm = await db.ProductModules.FindAsync(id); if (pm is null) return Results.NotFound(); db.ProductModules.Remove(pm); await db.SaveChangesAsync(); return Results.Ok();
        case "permissions":
            var p = await db.Permissions.FindAsync(id); if (p is null) return Results.NotFound(); db.Permissions.Remove(p); await db.SaveChangesAsync(); return Results.Ok();
        case "roles":
            var r = await db.Roles.FindAsync(id); if (r is null) return Results.NotFound(); db.Roles.Remove(r); await db.SaveChangesAsync(); return Results.Ok();
        case "role_permissions":
            var rp = await db.RolePermissions.FindAsync(id); if (rp is null) return Results.NotFound(); db.RolePermissions.Remove(rp); await db.SaveChangesAsync(); return Results.Ok();
        case "tenants":
            var t = await db.Tenants.FindAsync(id); if (t is null) return Results.NotFound(); db.Tenants.Remove(t); await db.SaveChangesAsync(); return Results.Ok();
        case "tenants_product_modules":
            var tpm = await db.TenantProductModules.FindAsync(id); if (tpm is null) return Results.NotFound(); db.TenantProductModules.Remove(tpm); await db.SaveChangesAsync(); return Results.Ok();
        case "users":
            var u = await db.Users.FindAsync(id); if (u is null) return Results.NotFound(); db.Users.Remove(u); await db.SaveChangesAsync(); return Results.Ok();
        case "user_roles":
            var ur = await db.UserRoles.FindAsync(id); if (ur is null) return Results.NotFound(); db.UserRoles.Remove(ur); await db.SaveChangesAsync(); return Results.Ok();
        case "usergroups":
            var ug = await db.UserGroups.FindAsync(id); if (ug is null) return Results.NotFound(); db.UserGroups.Remove(ug); await db.SaveChangesAsync(); return Results.Ok();
        case "organizations":
            var o = await db.Organizations.FindAsync(id); if (o is null) return Results.NotFound(); db.Organizations.Remove(o); await db.SaveChangesAsync(); return Results.Ok();
        case "operation_logs":
            var l = await db.OperationLogs.FindAsync(id); if (l is null) return Results.NotFound(); db.OperationLogs.Remove(l); await db.SaveChangesAsync(); return Results.Ok();
        default:
            return Results.NotFound();
    }
});

app.Run();
