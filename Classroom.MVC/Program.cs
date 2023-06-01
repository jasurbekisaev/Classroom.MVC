using Classroom.MVC.Helpers;
using ClassRoomData.Context;
using ClassRoomData.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// appdbcontextda yasalgan construktordan options keladi va bu yerda stringconnection yasaladi
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Server=sql.bsite.net\\MSSQL2016; " +
                        "Database=classroom12_db; " +
                        "User Id=classroom12_db; " +
                        "Password=asdqwe123;" +
                        "Encrypt=False;");
});



// identity build qilish, Identity regexlarni  yozib chiqamiz
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>(); // bu stores bizga identityni qoshish uchun foydalangan interfeyslarini databazaga qoshib beradi

// bitta respon tugaguncha userproviderdan 1 ta obyekt olinadi

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserProvider>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




// 31 mins left
