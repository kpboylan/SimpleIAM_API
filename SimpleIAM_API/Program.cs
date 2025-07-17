using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.Entity;
using SimpleIAM_API.Repository;
using SimpleIAM_API.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClinicalTrialDbContext>(options =>
    options.UseInMemoryDatabase("ClinicalTrialDb"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClinicalTrialDbContext>();

    if (!context.Users.Any())
    {
        var user = new User
        {
            Email = "johndoe@simpleiam.com",
            Password = "MyCats@123"
        };
        context.Users.Add(user);
        context.SaveChanges();
    }

    if (!context.Groups.Any())
    {
        context.Groups.AddRange(
            new Group { Name = "Investigator" },
            new Group { Name = "Subinvestigator" },
            new Group { Name = "Biostatistician" },
            new Group { Name = "Quality Assurance" },
            new Group { Name = "Sponsor" },
            new Group { Name = "Safety Officer" },
            new Group { Name = "Site Admin" },
            new Group { Name = "Study Monitor" }
        );

        context.SaveChanges();
    }
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
