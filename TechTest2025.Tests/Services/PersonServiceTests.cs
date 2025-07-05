using System.Collections.Generic;
using System.Linq;
using Moq;
using TechTest2025.Models;
using TechTest2025.Repositories;
using TechTest2025.Services;
using Xunit;

namespace TechTest2025.Tests.Services
{
    public class PersonServiceTests
    {
        [Fact]
        public void GetAllPersons_ReturnsAllPersonsFromRepository()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Address = "123 Main St", DateOfBirth = new DateTime(1990, 1, 1) },
                new Person { Id = 2, Name = "Bob", Address = "456 Elm St", DateOfBirth = new DateTime(1985, 5, 5) }
            }.AsQueryable();

            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(persons);

            var service = new PersonService(mockRepo.Object);

            // Act
            var result = service.GetAllPersons();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Alice");
            Assert.Contains(result, p => p.Name == "Bob");
        }
    }
}
