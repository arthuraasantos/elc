using Core.Entities.Users;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Files;
using Infra.Repositories;
using Core.Services;
using Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")), ServiceLifetime.Scoped);

builder.Services.AddScoped<IUnitofWork, UnitofWork>();
builder.Services.AddScoped<IMyFileRepository, FileRepository>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGradeService, GradeService>();


builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 4;

}).AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<TMContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Index";
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
