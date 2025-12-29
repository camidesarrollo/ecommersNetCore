using Ecommers.Application.Configuration;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

static async Task CrearUsuarioDePruebaAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AspNetUsers>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AspNetRoles>>(); // ✅ Cambiar a AspNetRoles

    // Crear rol Admin si no existe
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new AspNetRoles { Name = "Admin" });

    // Usuario de prueba
    string email = "test@correo.com";
    string password = "Admin123!";

    var usuario = await userManager.FindByEmailAsync(email);
    if (usuario == null)
    {
        usuario = new AspNetUsers
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            // ✅ AGREGAR CAMPOS REQUERIDOS
            FirstName = "Admin",
            LastName = "Test",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            TermsAccepted = true,
            PrivacyAccepted = true
        };

        var resultado = await userManager.CreateAsync(usuario, password);
        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(usuario, "Admin");
            Console.WriteLine($"✅ Usuario creado: {email} / {password}");
        }
        else
        {
            Console.WriteLine("❌ Error al crear usuario:");
            foreach (var error in resultado.Errors)
                Console.WriteLine($"   - {error.Description}");
        }
    }
    else
    {
        Console.WriteLine($"ℹ️  Usuario {email} ya existe");
    }
}

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Middlewares y rutas
app.ConfigureMiddlewares();

await CrearUsuarioDePruebaAsync(app);

app.Run();