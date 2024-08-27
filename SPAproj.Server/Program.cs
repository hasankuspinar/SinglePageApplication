using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SPAproj.Server.Data;
using SPAproj.Server.Models;
using SPAproj.Server.Repo;
using System.Configuration;

namespace SPAproj.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<PersonContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PersonContext") ?? throw new InvalidOperationException("Connection string 'PersonContext' not found.")));
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PersonContext")));

            // Add services to the container.
            

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserManager>();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ConfigurationService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var connectionString = builder.Configuration.GetConnectionString("PersonContext");
            builder.Services.AddDbContext<PersonContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "auth";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/api/auth/login";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseMiddleware<UserStatusMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

