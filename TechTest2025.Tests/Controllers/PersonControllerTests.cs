using Microsoft.AspNetCore.Mvc;
using Moq;
using TechTest2025.Controllers;
using TechTest2025.Models;
using TechTest2025.Services;

namespace TechTest2025.Tests.Controllers;

public class PersonControllerTests
{
    [Fact]
    public void GetAll_ReturnsOkWithAllPersons()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person { Id = 1, Name = "Alice", Address = "123 Main St", DateOfBirth = new DateTime(1990, 1, 1) },
            new Person { Id = 2, Name = "Bob", Address = "456 Elm St", DateOfBirth = new DateTime(1985, 5, 5) }
        }.AsQueryable();

        var mockService = new Mock<IPersonService>();
        mockService.Setup(s => s.GetAllPersons()).Returns(persons);

        var controller = new PersonController(mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPersons = Assert.IsAssignableFrom<IEnumerable<Person>>(okResult.Value);
        Assert.Equal(2, returnedPersons.Count());
    }
}
