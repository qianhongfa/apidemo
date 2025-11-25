# MiniApp (本地演示)

这是一个轻量级的演示程序，基于 ASP.NET Core minimal API + EF Core InMemory，提供针对多个实体的 REST CRUD 接口和静态前端页面。

运行步骤（在 PowerShell 中）:

```powershell
cd e:\test\apidemo\miniapp
dotnet restore
dotnet run
```

打开浏览器访问 `http://localhost:5000`（或控制台输出显示的端口）。

说明:
- 后端 API 统一为 `/api/{entity}`，支持 GET/POST/DELETE。部分 PUT 也有示例。
- 前端静态页面放在 `wwwroot/pages`，主页面为 `index.html`。
- 默认使用 InMemory 数据库，适合演示。若需连接 MySQL，可修改 `Program.cs` 中的 DbContext 配置并添加 MySQL EF Core 包。
