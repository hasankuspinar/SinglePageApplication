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
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserPassword> GetUserPassword(int userId)
        {
            return await _context.UserPasswords.SingleOrDefaultAsync(up => up.UserId == userId);
        }

        public async Task AddUser(User user, UserPassword userPassword, UserRole userRole, UserStatus userStatus)
        {
            await _context.Users.AddAsync(user);
            await _context.UserPasswords.AddAsync(userPassword);
            await _context.UserRoles.AddAsync(userRole);
            await _context.UserStatuses.AddAsync(userStatus);
            await _context.SaveChangesAsync();
        }
    }


