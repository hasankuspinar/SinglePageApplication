using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAproj.Server.Models;

namespace SPAproj.Server.Repo;

public interface IUserRepository
{
    Task<User> GetUserByUsername(string username);
    Task<UserPassword> GetUserPassword(int userId);
    Task AddUser(User user);
    Task AddUserPassword(UserPassword userPassword);
    Task AddUserRole(UserRole userRole);
    Task AddUserStatus(UserStatus userStatus);
    Task<UserRole> GetUserRole(int userId);
    Task<UserStatus> GetUserStatus(int userId);
}
