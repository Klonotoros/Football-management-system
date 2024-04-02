using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projekt.Data;
using Projekt.Models;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjektContext>(options =>
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())).UseSqlServer(builder.Configuration.GetConnectionString("ProjektContext") ?? throw new InvalidOperationException("Connection string 'ProjektContext' not found.")));

//kontener dependency injection
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) //sprawdzamy czy aktualne środowisko to development
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
//{
//    var context = serviceScope.ServiceProvider.GetRequiredService<ProjektContext>();
//    context.Database.EnsureCreated();
//    context.Team.Add(new Team() { Faculty = "w1" });
//    context.Team.Add(new Team() { Faculty = "w2" });
//    context.SaveChanges();
//    context.Match.Add(new Match() { HomeTeamId = 1, GuestTeamId = 2, Date = DateTime.Now });
//    context.SaveChanges();
//    var zm = context.Team.FirstOrDefault();
//    context.Team.Remove(zm);
//    context.SaveChanges();
//}


app.UseHttpsRedirection(); //przekierowanie na https
app.UseStaticFiles(); //jeżeli klient będzie odnosił się po pliki statyczne, to bedziemy w stanie je zwrócić do klienta

app.UseRouting(); //routing czyli dopasowywanie ścieżek do konkretnych handlerów

app.UseAuthorization(); //autoryzacja

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //domyślna ścieżka do kontrolera to home, do akcji to index, i opcjonalny parametr id

app.Run();
