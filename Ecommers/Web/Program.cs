using System.Formats.Asn1;
using Azure.Core;
using Ecommers.Application.Configuration;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

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

    //Crear datos iniciales de 
    var _configuracionService =
        scope.ServiceProvider.GetRequiredService<IConfiguracion>();
    var configuraciones = await _configuracionService.GetAllAsync();

    if(!configuraciones.Any())
    {
     
        var request = new ConfiguracionCreateRequest
        {
            AbreviacionEmpresa = "JPJ",
            NombreEmpresa = "Secos y Saludables JPJ",
            NombreAplicacion = "Secos y Saludables JPJ",
            Slogan = "Alimentos naturales para una vida saludable",

            IncluirEnSitemap = true,
            PrioridadSitemap = 1,

            ActivarCarrito = true,
            ActivarPagosOnline = false,

            Pais = "Chile",
            Ciudad = "Santiago",
            Direccion = "Av Grecia 2929",
            TelefonoContacto = "+56951108675",
            Whatsapp = "+56951108675",
            EmailContacto = "camila.zaragoza.villaseca@gmail.com",

            Moneda = "CL",
            SimboloMoneda = "$",
            Idioma = "es",

            ColorTemaNavegador = "Yellow",

            Logo = "img\\Configuraciones\\d08e0e7b-cc6e-4863-890c-86dc6cf25107.webp",
            Favicon = "img\\Configuraciones\\6426e523-eeed-40e1-9d16-b8b65bd89664.webp",
            MetaImagenPredeterminada = "img\\Configuraciones\\d08e0e7b-cc6e-4863-890c-86dc6cf25107.webp",

            Descripcion = "Tienda especializada en frutos secos, semillas, cereales, legumbres, especias y productos gourmet a granel y envasados. Ofrecemos una amplia variedad de alimentos naturales y seleccionados como almendras, nueces, pistachos, maní, quinoa, arroz basmati y arborio, lentejas, porotos, garbanzos, avena, harinas especiales, además de frutas deshidratadas y confitadas como dátiles, damascos, cranberries, pasas y mango.\r\n\r\nContamos también con aceitunas, aceites, miel, endulzantes naturales, chocolates, productos veganos, fermentados como chucrut, y una completa línea de especias y condimentos: canela, cardamomo, clavo de olor, nuez moscada, sal del Himalaya, mostaza y más. Complementamos nuestro catálogo con infusiones, tés, café, salsas y productos gourmet seleccionados.\r\n\r\nIdeal para quienes buscan alimentación saludable, ingredientes de calidad, productos naturales y opciones para cocina casera, repostería y gastronomía gourmet, con variedad, frescura y excelente relación precio-calidad.",

            Slug = "secos-y-saludables-jpj",

            /* =========================
               META SEO
            ========================= */
            MetaTitulo = "Secos y Saludables JPJ | Frutos secos y productos naturales",
            MetaDescripcion = "Frutos secos, semillas, legumbres, especias y productos gourmet a granel y envasados. Calidad, variedad y alimentación saludable.",
            MetaKeywords = "frutos secos, semillas, legumbres, productos naturales, alimentos saludables, frutos secos a granel, almendras, nueces, pistachos, maní, quinoa, arroz basmati, arroz arborio, lentejas, porotos, especias, condimentos, frutas deshidratadas, productos gourmet, tienda natural",
            MetaAutor = "Secos y Saludables JPJ",
            MetaCharset = "UTF-8",
            MetaLanguage = "es-CL",
            MetaViewport = "width=device-width, initial-scale=1.0",
            MetaCanonical = "https://www.secosysaludablesjpj.cl/",
            MetaRevisitAfter = "7 days",
            MetaRobots = "index, follow",
            MetaNoindex = false,
            MetaNofollow = false,

            /* =========================
               OPEN GRAPH (Facebook / WhatsApp)
            ========================= */
            OgTitulo = "Secos y Saludables JPJ | Frutos secos y productos naturales",
            OgDescripcion = "Tienda de frutos secos, semillas, legumbres y productos gourmet. Calidad, frescura y alimentación saludable.",
            OgTipo = "website",
            OgSitename = "Secos y Saludables JPJ",
            OgUrl = "https://www.secosysaludablesjpj.cl/",
            OgImagen = "https://www.secosysaludablesjpj.cl/img/logo.webp",

            /* =========================
               TWITTER / X
            ========================= */
            TwitterCard = "summary_large_image",
            TwitterTitulo = "Secos y Saludables JPJ | Alimentos naturales",
            TwitterDescripcion = "Frutos secos, semillas y productos gourmet a granel. Compra saludable y de calidad.",
            TwitterImagen = "https://www.secosysaludablesjpj.cl/img/logo.webp",
            TwitterSite = "@secosysaludablesjpj",
            TwitterCreator = "@secosysaludablesjpj",

            /* =========================
               REDES SOCIALES
            ========================= */
            Facebook = "https://www.facebook.com/secosysaludablesjpj",
            Instagram = "https://www.instagram.com/secosysaludablesjpj",
            Tiktok = "https://www.tiktok.com/@secosysaludablesjpj",
            Twitter = "https://x.com/secosysaludablesjpj",

            /* =========================
               SCHEMA.ORG
            ========================= */
            SchemaTipo = "Store",
            SchemaDatos = @"{
        ""@context"": ""https://schema.org"",
        ""@type"": ""Store"",
        ""name"": ""Secos y Saludables JPJ"",
        ""image"": ""https://www.secosysaludablesjpj.cl/img/logo.webp"",
        ""url"": ""https://www.secosysaludablesjpj.cl/"",
        ""telephone"": ""+56951108675"",
        ""address"": {
            ""@type"": ""PostalAddress"",
            ""streetAddress"": ""Av Grecia 2929"",
            ""addressLocality"": ""Santiago"",
            ""addressCountry"": ""CL""
        }
    }"
        };


        await _configuracionService.CreateAsync(request);
    }

    var _categorias =
       scope.ServiceProvider.GetRequiredService<ICategorias>();
    var categorias = await _categorias.GetAllActiveAsync();

    if (!categorias.Any())
    {
        var categoriasSeed = new List<CategoriaCreateRequest>
        {
            new()
            {
                Name = "Aceitunas y Olivas",
                ShortDescription = "Aceitunas y olivas seleccionadas, ideales para aperitivos, ensaladas y preparaciones gourmet.",
                Description = "Descubre nuestra variedad de aceitunas y olivas de alta calidad, perfectas para acompañar comidas, preparar aperitivos o realzar tus recetas con sabor mediterráneo.",
                Slug = "aceitunas-olivas",
                Image = "",
                BgClass = "bg-gradient-to-br from-lime-yellow to-magenta-strong",
                IsActive = true,
                SortOrder = 1
            },
            new()
            {
                Name = "Café, Té y Especias",
                ShortDescription = "Cafés, tés y especias aromáticas para disfrutar sabores intensos y naturales.",
                Description = "Explora nuestra selección de cafés, tés e especias cuidadosamente elegidas para ofrecer aromas, sabores y calidad que transforman cada momento y cada receta.",
                Slug = "cafe-te-especias",
                Image = "",
                BgClass = "bg-gradient-to-br from-golden-yellow to-nut-brown",
                IsActive = true,
                SortOrder = 2
            },
            new()
            {
                Name = "Cereales y Legumbres",
                ShortDescription = "Cereales y legumbres nutritivos, ideales para una alimentación equilibrada.",
                Description = "Encuentra cereales y legumbres de excelente calidad, ricos en nutrientes y perfectos para preparar platos saludables, caseros y llenos de energía.",
                Slug = "cereales-legumbres",
                Image = "",
                BgClass = "bg-gradient-to-br from-beige to-mint-green",
                IsActive = true,
                SortOrder = 3
            },
            new()
            {
                Name = "Chocolates y Dulces",
                ShortDescription = "Chocolates y dulces irresistibles para disfrutar y compartir.",
                Description = "Deléitate con nuestra variedad de chocolates y dulces, elaborados para satisfacer los antojos y acompañar momentos especiales con un toque dulce.",
                Slug = "chocolates-dulces",
                Image = "",
                BgClass = "bg-gradient-to-br from-golden-yellow to-nut-brown",
                IsActive = true,
                SortOrder = 4
            },
            new()
            {
                Name = "Conservas y Vegetales",
                ShortDescription = "Conservas y vegetales listos para complementar tus comidas.",
                Description = "Nuestra categoría de conservas y vegetales ofrece productos prácticos y sabrosos, ideales para ahorrar tiempo sin perder calidad ni sabor.",
                Slug = "conservas-vegetales",
                Image = "",
                BgClass = "bg-gradient-to-br from-beige to-mint-green",
                IsActive = true,
                SortOrder = 5
            },
            new()
            {
                Name = "Frutas Deshidratadas",
                ShortDescription = "Frutas deshidratadas naturales, dulces y llenas de energía.",
                Description = "Disfruta frutas deshidratadas cuidadosamente procesadas para conservar su sabor y nutrientes, perfectas como snack o ingrediente para recetas.",
                Slug = "frutas-deshidratadas",
                Image = "",
                BgClass = "bg-gradient-to-br from-beige to-golden-yellow",
                IsActive = true,
                SortOrder = 6
            },
            new()
            {
                Name = "Frutos Secos",
                ShortDescription = "Frutos secos seleccionados, nutritivos y llenos de sabor.",
                Description = "Nuestra selección de frutos secos ofrece calidad y frescura, ideales para consumir como snack, en postres o como parte de una dieta saludable.",
                Slug = "frutos-secos",
                Image = "",
                BgClass = "bg-gradient-to-br from-nut-brown to-golden-yellow",
                IsActive = true,
                SortOrder = 7
            },
            new()
            {
                Name = "Harinas y Preparación",
                ShortDescription = "Harinas y mezclas listas para cocinar y hornear.",
                Description = "Encuentra harinas y productos de preparación ideales para panadería y cocina casera, pensados para facilitar tus recetas diarias.",
                Slug = "harinas-preparacion",
                Image = "",
                BgClass = "bg-gradient-to-br from-beige to-golden-yellow",
                IsActive = true,
                SortOrder = 8
            },
            new()
            {
                Name = "Snacks y Otros",
                ShortDescription = "Snacks variados y productos ideales para cualquier momento del día.",
                Description = "Descubre nuestra categoría de snacks y otros productos, pensados para acompañarte en cualquier ocasión con opciones prácticas y sabrosas.",
                Slug = "snacks-y-otros",
                Image = "",
                BgClass = "bg-gradient-to-br from-beige to-golden-yellow",
                IsActive = true,
                SortOrder = 9
            }
        };


        foreach (var categoria in categoriasSeed)
        {
            await _categorias.CreateAsync(categoria);
        }

        Console.WriteLine("✅ Categorías iniciales creadas");
    }
    else
    {
        Console.WriteLine("ℹ️  Las categorías ya existen");
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