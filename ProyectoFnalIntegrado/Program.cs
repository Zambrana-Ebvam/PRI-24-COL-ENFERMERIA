using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoFnalIntegrado.Data;
using ProyectoFnalIntegrado.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la cadena de conexi�n global
AppSettings.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configuraci�n del DbContext para la aplicaci�n
builder.Services.AddDbContext<DaContext>(options =>
    options.UseSqlServer(AppSettings.ConnectionString));

// Configuraci�n de autenticaci�n basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Configuraci�n de sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar servicios personalizados, como servicio de correo
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes HTTP
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar autenticaci�n, sesiones y autorizaci�n
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

// Configurar rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
