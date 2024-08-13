using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SPAproj.Server.Repo;
using SPAproj.Server.Models;
using Moq;
using System.Security.Cryptography;
using System.Text;
using System.Data.Common;
using SPAproj.Server.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MStest
{
    [TestClass]
    public class UserManagerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<AppDbContext> _contextMock;
        private Mock<DatabaseFacade> _databaseMock;
        private UserManager _userManager;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _databaseMock = new Mock<DatabaseFacade>(_contextMock.Object);

            _contextMock.Setup(m => m.Database).Returns(_databaseMock.Object);
            _databaseMock.Setup(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Mock.Of<IDbContextTransaction>);

            _userManager = new UserManager(_userRepositoryMock.Object, _contextMock.Object);
        }

        [TestMethod]
        public async Task Register_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var username = "existingUser";
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username))
                               .ReturnsAsync(new User { Username = username });

            // Act
            var result = await _userManager.Register(username, "password", "role", 1);

            // Assert
            Assert.IsFalse(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task Register_ShouldReturnTrue_WhenNewUserIsAdded()
        {
            // Arrange
            var username = "newUser";
            var userId = 1;

            _userRepositoryMock.SetupSequence(repo => repo.GetUserByUsername(username))
                               .ReturnsAsync((User)null)  // First call returns null (user doesn't exist)
                               .ReturnsAsync(new User { UserId = userId, Username = username }); // Second call returns the newly added user

            _contextMock.Setup(context => context.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Mock.Of<IDbContextTransaction>);

            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>()))
                               .Callback<User>(user => user.UserId = userId); // Assign an ID when adding a new user

            // Act
            var result = await _userManager.Register(username, "password", "role", 1);

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Exactly(2));
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUserPassword(It.IsAny<UserPassword>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUserRole(It.IsAny<UserRole>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUserStatus(It.IsAny<UserStatus>()), Times.Once);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonExistentUser";
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _userManager.Login(username, "password");

            // Assert
            Assert.IsFalse(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Once);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var username = "user";
            var user = new User { UserId = 1, Username = username };
            var incorrectPassword = "incorrectPassword";

            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username))
                               .ReturnsAsync(user);

            _userRepositoryMock.Setup(repo => repo.GetUserPassword(user.UserId))
                               .ReturnsAsync(new UserPassword { UserId = user.UserId, Password = "hashedPassword" });

            // Act
            var result = await _userManager.Login(username, incorrectPassword);

            // Assert
            Assert.IsFalse(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserPassword(user.UserId), Times.Once);
        }

        [TestMethod]
        public async Task Login_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var username = "user";
            var password = "correctPassword";
            var user = new User { UserId = 1, Username = username };
            var combinedPassword = $"{user.UserId}{username}{password}";
            var hashedPassword = ComputeSha512Hash(combinedPassword);

            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username))
                               .ReturnsAsync(user);

            _userRepositoryMock.Setup(repo => repo.GetUserPassword(user.UserId))
                               .ReturnsAsync(new UserPassword { UserId = user.UserId, Password = hashedPassword });

            // Act
            var result = await _userManager.Login(username, password);

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserPassword(user.UserId), Times.Once);
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
    }





}