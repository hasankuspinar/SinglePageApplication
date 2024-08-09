using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAproj.Server.Models;

namespace SPAproj.Server.Repo;

public class UserManager
{
    private readonly IUserRepository _userRepository;

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
        var newUserPassword = new UserPassword { Password = password };
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

        var userPassword = await _userRepository.GetUserPassword(user.UserId);
        if (userPassword == null || userPassword.Password != password)
            return false;

        return true;
    }
}
