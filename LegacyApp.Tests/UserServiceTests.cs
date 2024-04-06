using Moq;
using System;
using Xunit;

namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IClientRepository> _mockClientRepository = new Mock<IClientRepository>();
        private readonly Mock<IUserCreditService> _mockUserCreditService = new Mock<IUserCreditService>();

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void AddUser_ValidData_ReturnsTrue()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var dateOfBirth = new DateTime(1990, 1, 1);
            var clientId = 1;

            _mockClientRepository.Setup(x => x.GetClientById(clientId)).Returns(new Client
            {
                Type = "NormalClient"
            });

            _mockUserCreditService.Setup(x => x.GetCreditLimit(lastName, dateOfBirth)).Returns(700);

            // Act
            var result = _userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddUser_UnderAge_ReturnsFalse()
        {
            // Arrange
            var firstName = "Young";
            var lastName = "User";
            var email = "young.user@example.com";
            var dateOfBirth = DateTime.Now.AddYears(-20); // Making the user 20 years old
            var clientId = 1;

            // Act
            var result = _userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddUser_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "invalidemail";
            var dateOfBirth = new DateTime(1990, 1, 1);
            var clientId = 1;

            // Act
            var result = _userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void AddUser_VeryImportantClient_NoCreditLimitCheck()
        {
            // Arrange
            var firstName = "VIP";
            var lastName = "Client";
            var email = "vip.client@example.com";
            var dateOfBirth = new DateTime(1990, 1, 1);
            var clientId = 2; // Assuming this ID corresponds to a VeryImportantClient

            _mockClientRepository.Setup(x => x.GetClientById(clientId)).Returns(new Client
            {
                Type = "VeryImportantClient"
            });

            // Act
            var result = _userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

            // Assert
            Assert.True(result);
            _mockUserCreditService.Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public void AddUser_NonexistentClient_ReturnsFalse()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var dateOfBirth = new DateTime(1980, 1, 1);
            var clientId = 999; // ID that does not exist

            _mockClientRepository.Setup(x => x.GetClientById(clientId)).Throws(new ArgumentException("Client does not exist"));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _userService.AddUser(firstName, lastName, email, dateOfBirth, clientId));
        }

    }
}
