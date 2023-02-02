using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;

namespace Studentenbeheer.Data
{
    public static class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<StudentenbeheerUser> userManager)
        {
            using var context = new IdentityContext(
                serviceProvider.GetRequiredService<DbContextOptions<IdentityContext>>());
            {
                StudentenbeheerUser user = null;

                if (!context.Users.Any())
                {
                    StudentenbeheerUser dummy = new StudentenbeheerUser { Id = "-", FirstName = "-", LastName = "-", UserName = "-", Email = "?@?.?" };
                    context.Users.Add(dummy);
                    context.SaveChanges();
                    user = new StudentenbeheerUser
                    {
                        FirstName = "System",
                        LastName = "Administrator",
                        UserName = "Admin",
                        Email = "System.Administrator@GroupSpace.be",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(user, "Abc.12345");
                }
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "Docent" },
                        new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "Student" },
                        new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "Admin" });
                    context.SaveChanges();
                }
                if (!context.Gender.Any())
                {
                    context.Gender.AddRange(
                    new Models.Gender
                    {
                        Id = '?',
                        Name = "?"
                    },
                   new Models.Gender
                   {
                       Id = 'M',
                       Name = "Male"
                   },
                   new Models.Gender
                   {
                       Id = 'F',
                       Name = "Female",
                       

                   });
                    context.SaveChanges();
                }
                if (!context.Student.Any())
                {
           
                context.Student.AddRange(

                   new Models.Student
                   {
                       Voornaam = "?",
                       Achternaam = "?",
                       Geboortedatum = DateTime.MinValue,
                       GenderID = '?',
                       UserId = "-"
                   },
                   new Models.Student
                   {
                       Voornaam = "soufiane",
                       Achternaam = "Hamoumi",
                       Geboortedatum = DateTime.MinValue,
                       GenderID = 'M',
                       UserId = "-"
                   }) ;
                context.SaveChanges();
                }
                if (!context.Module.Any())
                {
                    context.Module.AddRange(
                        new Models.Module
                        {
                            Name = "?",
                            Omschrijving = "?",


                        },
                        new Models.Module
                        {
                            Name = "Module1",
                            Omschrijving = "Errste Module",


                        });
                    context.SaveChanges();
                    // Docent
                }
                    if (!context.Docent.Any())
                {
                    context.Docent.AddRange(
                        new Models.Docent
                        {
                            Voornaam = "?",
                            Achternaam = "?",
                            Geboortedatum= DateTime.MinValue,
                            GenderID = 'M',
                            Deleted = DateTime.MaxValue,
                            UserId = "-"


                        });
                    context.SaveChanges();
                }



                if (!context.Inschrijvingen.Any())
                {
                    context.Inschrijvingen.AddRange(
                        new Models.Inschrijvingen
                        {
                            ModuleId = 1,
                            StudentId = 1,
                            InschrijvingsDatum = DateTime.MinValue,
                            AfgelegdOp  = DateTime.MaxValue,
                            Resultaat = "?"
                        },
                        new Models.Inschrijvingen
                        {
                            ModuleId = 2,
                            StudentId = 2,
                            InschrijvingsDatum = DateTime.MinValue,
                            AfgelegdOp = DateTime.MaxValue,
                            Resultaat = "Resultaat"
                        });
                    context.SaveChanges();
                }
                if (user != null)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "Docent" },
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "Student" },
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "Admin" });
                    context.SaveChanges();
                }
            }
        }
    }
}
