using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
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

// Extraemos el Project ID desde appsettings.json
var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];

if (FirebaseApp.DefaultInstance == null)
{
    // Construimos la ruta absoluta al archivo JSON
    string pathCredenciales = Path.Combine(builder.Environment.ContentRootPath, "firebase-key.json");

    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(pathCredenciales), 
        ProjectId = firebaseProjectId
    });
}


builder.Services.AddControllersWithViews()
.AddJsonOptions(options =>
 {
     // Esta línea le dice a la API: "Si recibes un texto, conviértelo al Enum correspondiente"
     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
 });
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()   // Permite cualquier IP, dominio o localhost
              .AllowAnyHeader()   // Permite que pase el JWT de Firebase
              .AllowAnyMethod();  // Permite GET, POST, PUT, DELETE, OPTIONS
    });
});


// Agregamos los esquemas extra (JWT y Cookies Públicas) SIN pisar el default de Identity.
builder.Services.AddAuthentication()
    // 1. Esquema JWT (Para que la App Móvil hable con la API)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    })
    // 2. Esquema Cookies Públicas (Para que los abuelos naveguen la Web)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

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


app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("PermitirTodo");
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated != true)
    {
        var result = await context.AuthenticateAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        if (result.Succeeded && result.Principal != null)
        {
            context.User = result.Principal;
        }
    }
    await next();
});
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

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
