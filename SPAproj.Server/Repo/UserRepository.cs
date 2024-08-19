using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAproj.Server.Models;
using SPAproj.Server.Data;

namespace SPAproj.Server.Repo;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _context.User.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserPassword> GetUserPassword(int userId)
    {
        return await _context.UserPassword.SingleOrDefaultAsync(up => up.UserId == userId);
    }
    public async Task<UserRole> GetUserRole(int userId)
    {
        return await _context.UserRole.SingleOrDefaultAsync(ur => ur.UserId == userId); ; 
    }
    public async Task AddUser(User user)
    {
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserPassword(UserPassword userPassword)
    {
        await _context.UserPassword.AddAsync(userPassword);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserRole(UserRole userRole)
    {
        await _context.UserRole.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserStatus(UserStatus userStatus)
    {
        await _context.UserStatus.AddAsync(userStatus);
        await _context.SaveChangesAsync();
    }

        
}


