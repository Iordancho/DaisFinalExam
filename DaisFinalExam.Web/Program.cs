using DaisFinalExam.Repository;
using DaisFinalExam.Repository.Implementations.Account;
using DaisFinalExam.Repository.Implementations.Payment;
using DaisFinalExam.Repository.Implementations.User;
using DaisFinalExam.Repository.Implementations.UserAccount;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.Payment;
using DaisFinalExam.Repository.Interfaces.User;
using DaisFinalExam.Repository.Interfaces.UserAccount;
using DaisFinalExam.Services.Implementations.Account;
using DaisFinalExam.Services.Implementations.Authentication;
using DaisFinalExam.Services.Implementations.Payment;
using DaisFinalExam.Services.Implementations.User;
using DaisFinalExam.Services.Implementations.UserAccount;
using DaisFinalExam.Services.Interfaces.Account;
using DaisFinalExam.Services.Interfaces.Authentication;
using DaisFinalExam.Services.Interfaces.Payment;
using DaisFinalExam.Services.Interfaces.User;
using DaisFinalExam.Services.Interfaces.UserAccount;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IUserService, UserService>();




// Register repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



ConnectionFactory.Initialize(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
