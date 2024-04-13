using AspNetCoreHero.ToastNotification;
using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using StudentInformationManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var conn = builder.Configuration.GetConnectionString("SchoolManagementDbConnection");
builder.Services.AddDbContext<SchoolManagementDbContext>(q => q.UseSqlServer(conn));

builder.Services.AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
    });
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddNotyf(c => {
    c.DurationInSeconds = 5;
    c.IsDismissable = true;
    c.Position = NotyfPosition.TopRight;
});

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
