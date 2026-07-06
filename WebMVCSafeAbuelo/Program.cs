using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<WebMVCSafeAbuelo.Services.IIncidenteService, WebMVCSafeAbuelo.Services.IncidenteService>();
builder.Services.AddScoped<IMetodologiaService, MetodologiaService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<WebMVCSafeAbuelo.Models.UsuarioAdministrador>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- FIN DEL SCRIPT ---

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
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
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); 
        var userManager = services.GetRequiredService<UserManager<WebMVCSafeAbuelo.Models.UsuarioAdministrador>>();
        var context = services.GetRequiredService<ApplicationDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();

        // Ejecutamos el inicializador de forma asíncrona
        await WebMVCSafeAbuelo.Data.DbInitializer.InitializeAsync(userManager, context, configuration);
    }
    catch (Exception ex)
    {
        // En caso de fallar, registramos el error en la consola de depuración
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error irreversible durante el sembrado de datos.");
    }
}

app.Run(); 
