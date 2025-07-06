using TechTest2025.Repositories;

namespace TechTest2025.Tests.Repositories;
//In most cases it doesn't make sense to unit test Repository layer unless it has complex logic.
//But since we are loading data from a file some basic sanity testing is required
public class PersonRepositoryTests : IDisposable
{
    private readonly string _testFilePath;

    public PersonRepositoryTests()
    {
        // Create a unique temp file for each test run
        _testFilePath = Path.GetTempFileName();
    }

    [Fact]
    public void GetAll_FileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        File.Delete(_testFilePath); // Ensure file does not exist
        var repo = new PersonRepository(_testFilePath);

        // Act
        var result = repo.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_FileWithValidLines_ReturnsParsedPersons()
    {
        // Arrange
        var lines = new[]
        {
            "Alice,123 Main St,1990-01-01",
            "Bob,456 Elm St,1985-05-05"
        };
        File.WriteAllLines(_testFilePath, lines);
        var repo = new PersonRepository(_testFilePath);

        // Act
        var result = repo.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Alice", result[0].Name);
        Assert.Equal("Bob", result[1].Name);
        Assert.Equal("123 Main St", result[0].Address);
        Assert.Equal(new DateTime(1990, 1, 1), result[0].DateOfBirth);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public void GetAll_FileWithInvalidLine_SkipsInvalidLine()
    {
        // Arrange
        var lines = new[]
        {
            "Alice,123 Main St,1990-01-01",
            "InvalidLineWithoutCommas",
            "Bob,456 Elm St,1985-05-05"
        };
        File.WriteAllLines(_testFilePath, lines);
        var repo = new PersonRepository(_testFilePath);

        // Act
        var result = repo.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Alice");
        Assert.Contains(result, p => p.Name == "Bob");
    }

    [Fact]
    public void GetAll_FileWithInvalidDate_SkipsInvalidLine()
    {
        // Arrange
        var lines = new[]
        {
            "Alice,123 Main St,not-a-date",
            "Bob,456 Elm St,1985-05-05"
        };
        File.WriteAllLines(_testFilePath, lines);
        var repo = new PersonRepository(_testFilePath);

        // Act
        var result = repo.GetAll().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Bob", result[0].Name);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }
}
