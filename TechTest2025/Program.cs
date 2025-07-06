using TechTest2025.Repositories;
using TechTest2025.Services;

namespace TechTest2025;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //lowercase url
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        //global exception handler prevents app from crashing in the event of unhandled exception
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        // Dependency injection
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/people.txt");
        builder.Services.AddSingleton<IPersonRepository>(new PersonRepository(filePath));
        builder.Services.AddScoped<IPersonService, PersonService>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}