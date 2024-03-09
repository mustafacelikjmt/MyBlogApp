using BootcampFinalProject.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BootcampFinalProject.Data.Concrete.EfCore
{
    public static class SeedData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "admin123";
        private const string modUser = "wade";
        private const string modPassword = "mod123";

        public static async Task SeedDataBlog(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BlogContext>();
            if (context != null)
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                var user = await userManager.FindByNameAsync(adminUser);
                var user2 = await userManager.FindByNameAsync(modUser);
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new AppRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("Moderator"))
                {
                    await roleManager.CreateAsync(new AppRole("Moderator"));
                }
                if (user == null)
                {
                    user = new AppUser
                    {
                        FullName = "Mustafa Çelik",
                        UserName = adminUser,
                        Email = "mustafacelik2001mc2001@gmail.com",
                        PhoneNumber = "05342310587",
                        Image = "person-4.jpg",
                        EmailConfirmed = true
                    };
                    IdentityResult result = await userManager.CreateAsync(user, adminPassword);
                    if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    if (!result.Succeeded)
                    {
                        // Hataları günlüğe kaydet
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
                if (user2 == null)
                {
                    user2 = new AppUser
                    {
                        FullName = "Wade Warren",
                        UserName = modUser,
                        Email = "info@wade.com",
                        PhoneNumber = "05451544122",
                        Image = "person-2.jpg",
                        EmailConfirmed = true
                    };
                    IdentityResult result2 = await userManager.CreateAsync(user2, modPassword);
                    if (user2 != null && !await userManager.IsInRoleAsync(user2, "Moderator"))
                    {
                        await userManager.AddToRoleAsync(user2, "Moderator");
                    }
                    if (!result2.Succeeded)
                    {
                        // Hataları günlüğe kaydet
                        foreach (var error in result2.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Software ", Url = "software", Color = TagColor.success },
                        new Tag { Text = "Game", Url = "game", Color = TagColor.success },
                        new Tag { Text = "Business", Url = "business", Color = TagColor.success },
                        new Tag { Text = "Culture", Url = "culture", Color = TagColor.success },
                        new Tag { Text = "Sport", Url = "sport", Color = TagColor.success },
                        new Tag { Text = "Food", Url = "food", Color = TagColor.success },
                        new Tag { Text = "Politics", Url = "politics", Color = TagColor.success },
                        new Tag { Text = "Celebrity", Url = "celebrity", Color = TagColor.success },
                        new Tag { Text = "Startups", Url = "startups", Color = TagColor.success },
                        new Tag { Text = "Travel", Url = "travel", Color = TagColor.success });
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "What is the son of Football Coach John Gruden, Deuce Gruden doing Now?",
                            Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Distinctio placeat exercitationem magni voluptates dolore. Tenetur fugiat voluptates quas.",
                            Url = "single",
                            Content = "Burası content (içerik) bölümü",
                            Image = "post-landscape-6.jpg",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-3),
                            Tags = context.Tags.Where(x => x.Text == "Business").ToList(),
                            UserId = user2.UserId,
                            Comments = new List<Comment>
                            {
                                new Comment{Text="Güzel bir blog.",PublishedOn = DateTime.Now.AddDays(-10), UserId=user.UserId},
                            }
                        },
                        new Post
                        {
                            Title = "Asp Net Core",
                            Description = "Asp Net Core dersleri",
                            Url = "aspnet-core",
                            Content = "Asp Net Core Dersleri",
                            Image = "post-landscape-1.jpg",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-7),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = user.UserId,
                            Comments = new List<Comment>
                            {
                                new Comment{Text="Güzel bir blog.",PublishedOn = DateTime.Now.AddDays(-10), UserId=user.UserId},
                            }
                        },
                        new Post
                        {
                            Title = "Unity Game",
                            Description = "Unity Game dersleri",
                            Url = "unity-game",
                            Content = "Unity Game Dersleri",
                            Image = "post-landscape-2.jpg",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Skip(1).Take(3).ToList(),
                            UserId = user.UserId,
                            Comments = new List<Comment>
                            {
                                new Comment{Text="Mutlaka okuyun.",PublishedOn = new DateTime(), UserId=user.UserId}
                            }
                        },
                        new Post
                        {
                            Title = "Backend",
                            Description = "Backend dersleri",
                            Url = "backend",
                            Content = "Backend Dersleri",
                            Image = "post-landscape-3.jpg",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-12),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = user.UserId
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}
