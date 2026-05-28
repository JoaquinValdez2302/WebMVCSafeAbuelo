using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<WebMVCSafeAbuelo.Models.UsuarioAdministrador>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Solicitamos las herramientas necesarias al contenedor de dependencias
        var userManager = services.GetRequiredService<UserManager<WebMVCSafeAbuelo.Models.UsuarioAdministrador>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        // Ejecutamos el inicializador de forma asíncrona
        await WebMVCSafeAbuelo.Data.DbInitializer.InitializeAsync(userManager, configuration);
    }
    catch (Exception ex)
    {
        // En caso de fallar, registramos el error en la consola de depuración
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error irreversible durante el sembrado de datos.");
    }
}

app.Run(); 
