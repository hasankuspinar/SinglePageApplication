using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SPAproj.Server.Repo;
using SPAproj.Server.Models;
using Moq;

namespace MStest
{
    [TestClass]
    public class UserManagerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserManager _userManager;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userManager = new UserManager(_userRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Register_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userManager.Register("newuser", "password", "role", 1);

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>(), It.IsAny<UserPassword>(), It.IsAny<UserRole>(), It.IsAny<UserStatus>()), Times.Once);
        }

        [TestMethod]
        public async Task Register_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var existingUser = new User { Username = "existinguser" };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userManager.Register("existinguser", "password", "role", 1);

            // Assert
            Assert.IsFalse(result);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>(), It.IsAny<UserPassword>(), It.IsAny<UserRole>(), It.IsAny<UserStatus>()), Times.Never);
        }

        [TestMethod]
        public async Task Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var existingUser = new User { Username = "validuser", UserId = 1 };
            var existingUserPassword = new UserPassword { UserId = 1, Password = "password" };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.GetUserPassword(It.IsAny<int>()))
                .ReturnsAsync(existingUserPassword);

            // Act
            var result = await _userManager.Login("validuser", "password");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userManager.Login("nonexistentuser", "password");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var existingUser = new User { Username = "validuser", UserId = 1 };
            var existingUserPassword = new UserPassword { UserId = 1, Password = "correctpassword" };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.GetUserPassword(It.IsAny<int>()))
                .ReturnsAsync(existingUserPassword);

            // Act
            var result = await _userManager.Login("validuser", "wrongpassword");

            // Assert
            Assert.IsFalse(result);
        }
    }
    


}