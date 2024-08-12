using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAproj.Server.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace SPAproj.Server.Repo;

public class UserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Register(string username, string password, string role, int status)
    {
        var existingUser = await _userRepository.GetUserByUsername(username);
        if (existingUser != null)
            return false;

        var newUser = new User { Username = username };
        var combinedPassword = $"{username}{password}";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(combinedPassword);
        var newUserPassword = new UserPassword { Password = hashedPassword };
        var newUserRole = new UserRole { Role = role };
        var newUserStatus = new UserStatus { Status = status };

        await _userRepository.AddUser(newUser, newUserPassword, newUserRole, newUserStatus);
        return true;
    }

    public async Task<bool> Login(string username, string password)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
            return false;
        var combinedPassword = $"{username}{password}";
        var userPassword = await _userRepository.GetUserPassword(user.UserId);
        if (!BCrypt.Net.BCrypt.Verify(combinedPassword,userPassword.Password))
            return false;

        return true;
    }
}
