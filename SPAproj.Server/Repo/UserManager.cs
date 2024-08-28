using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAproj.Server.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using SPAproj.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SPAproj.Server.Repo;

public class UserManager
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    public UserManager(IUserRepository userRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<bool> Register(string username, string password, string role, int status)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingUser = await _userRepository.GetUserByUsername(username);
                if (existingUser != null)
                    return false;

                var newUser = new User { Username = username };
                await _userRepository.AddUser(newUser);

                var user = await _userRepository.GetUserByUsername(username);
                var combinedPassword = $"{user.UserId}{username}{password}";
                var hashedPassword = ComputeSha512Hash(combinedPassword);
                var newUserPassword = new UserPassword { UserId = user.UserId, Password = hashedPassword };
                await _userRepository.AddUserPassword(newUserPassword);

                var newUserRole = new UserRole { UserId = user.UserId, Role = role };
                await _userRepository.AddUserRole(newUserRole);

                var newUserStatus = new UserStatus { UserId = user.UserId, Status = status };
                await _userRepository.AddUserStatus(newUserStatus);

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> Login(string username, string password, HttpContext httpContext)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
            return false;

        var combinedPassword = $"{user.UserId}{username}{password}";
        var userPassword = await _userRepository.GetUserPassword(user.UserId);
        var hashedPassword = ComputeSha512Hash(combinedPassword);

        if (userPassword.Password != hashedPassword)
            return false;

        var role = await _userRepository.GetUserRole(user.UserId);
        var userStatus = await _userRepository.GetUserStatus(user.UserId);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, role.Role),
            new Claim("UserStatus", userStatus.Status.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
        };

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


        return true;
    }
    public bool IsUserLoggedIn(HttpContext httpContext)
    {
        return httpContext.User.Identity.IsAuthenticated;
    }

    private string ComputeSha512Hash(string input)
    {
        using (var sha512 = SHA512.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public async Task<bool> UpdateUserStatus(string username, int newStatus)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return false;
        }

        var userStatus = await _context.UserStatus.FirstOrDefaultAsync(us => us.UserId == user.UserId);
        if (userStatus != null)
        {
            userStatus.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }



}
