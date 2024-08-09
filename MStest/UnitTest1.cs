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
    /*public class UserManagerIntegrationTests
    {
        private DbContextOptions<AppDbContext> _options;
        private AppDbContext _context;
        private UserRepository _userRepository;
        private UserManager _userManager;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the DbContext options to use an in-memory database
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create a new context for each test to ensure a clean state
            _context = new AppDbContext(_options);
            _userRepository = new UserRepository(_context);
            _userManager = new UserManager(_userRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Ensure the database is cleaned up after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestInitialize]
        public void Setup()
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Initialize the DbContext options to use a local database
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create a new context for each test to ensure a clean state
            _context = new AppDbContext(_options);
            _userRepository = new UserRepository(_context);
            _userManager = new UserManager(_userRepository);

            // Ensure the database is clean before each test 
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Ensure the database is cleaned up after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task Register_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            // Act
            var result = await _userManager.Register("newuser", "password", "role", 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, _context.Users.Count());
        }

        [TestMethod]
        public async Task Register_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var existingUser = new User { Username = "existinguser" };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            // Act
            var result = await _userManager.Register("existinguser", "password", "role", 1);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, _context.Users.Count());
        }

        [TestMethod]
        public async Task Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var existingUser = new User { Username = "validuser" };
            var existingUserPassword = new UserPassword { UserId = existingUser.UserId, Password = "password" };
            _context.Users.Add(existingUser);
            _context.UserPasswords.Add(existingUserPassword);
            _context.SaveChanges();

            // Act
            var result = await _userManager.Login("validuser", "password");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Act
            var result = await _userManager.Login("nonexistentuser", "password");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var existingUser = new User { Username = "validuser" };
            var existingUserPassword = new UserPassword { UserId = existingUser.UserId, Password = "correctpassword" };
            _context.Users.Add(existingUser);
            _context.UserPasswords.Add(existingUserPassword);
            _context.SaveChanges();

            // Act
            var result = await _userManager.Login("validuser", "wrongpassword");

            // Assert
            Assert.IsFalse(result);
        }
    }*/


}