using BookStore.BL.Auth;
using BookStore.BL.Books;
using BookStore.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();     // For Swagger usage
builder.Services.AddSwaggerGen();               // For Swagger usage

// PostgreSQL
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// BL
builder.Services.AddScoped<IBooksManager, BooksProvider>();
builder.Services.AddScoped<IAuthProvider, AuthProvider>();

var app = builder.Build();

// Swagger middleware (add BEFORE MapControllerRoute)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
