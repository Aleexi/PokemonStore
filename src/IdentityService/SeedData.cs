using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // If users already exists, don't seed database
        if (userMgr.Users.Any()) return;

        var alice = userMgr.FindByNameAsync("alice").Result;
        if (alice == null)
        {
            // If user doesn't exists, add it to the database
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Add claims to the user
            result = userMgr.AddClaimsAsync(alice, new List<Claim>
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("alice created, claims added to alice...");
        }
        else
        {
            Log.Debug("alice already exists");
        }

        var alex = userMgr.FindByNameAsync("alex").Result;
        if (alex == null)
        {
            // If user doesn't exists, add it to the database
            alex = new ApplicationUser
            {
                UserName = "alex",
                Email = "AlexNeumann@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(alex, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // Add claims to the user
            result = userMgr.AddClaimsAsync(alex, new List<Claim>
                        {
                            new Claim(JwtClaimTypes.Name, "Alex Neumann")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("alex created, claims added to alex...");
        }
        else
        {
            Log.Debug("alex already exists");
        }

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith")
                            
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("bob created");
        }
        else
        {
            Log.Debug("bob already exists");
        }
    }
}
