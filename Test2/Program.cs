using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test2.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Test2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Test2Context") ?? throw new InvalidOperationException("Connection string 'Test2Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "employee",
    pattern: "Employee/{action}/{id?}",
    defaults: new { controller = "Employee", action = "Index" });
app.MapControllerRoute(
    name: "invite",
    pattern: "Email/{action}/{id?}",
    defaults: new { controller = "Email", action = "Index" });
app.MapControllerRoute(
    name: "history",
    pattern: "Email/History",
    defaults: new { controller = "Email", action = "InvitationHistory" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
