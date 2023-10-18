using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StaffAttendanceSystem.Data;
using Microsoft.AspNetCore.Identity;
using StaffAttendanceSystem.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

int[] a;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Teacher Attendance", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("SqlConnection")));

//builder.Services.AddDefaultIdentity<StaffAttendanceSystemUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<StaffAttendanceSystemContext>();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<StaffAttendanceSystemContext>();




builder.Services.AddIdentity<StaffAttendanceSystemUser, IdentityRole>()
            .AddEntityFrameworkStores<StaffAttendanceSystemContext>()
            .AddDefaultTokenProviders().AddDefaultUI();

builder.Services.AddDbContext<StaffAttendanceSystemContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("SqlConnection")));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teacher Attendance");
    c.RoutePrefix = "swagger"; // Set a separate route for Swagger UI
});

app.Run();
