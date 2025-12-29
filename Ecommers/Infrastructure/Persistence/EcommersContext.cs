using System;
using System.Collections.Generic;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Infrastructure.Persistence;

public class EcommersContext : IdentityDbContext<
       AspNetUsers,
       AspNetRoles,
       string,
       IdentityUserClaim<string>,
       IdentityUserRole<string>,
       IdentityUserLogin<string>,
       IdentityRoleClaim<string>,
       IdentityUserToken<string>>
{
    public EcommersContext()
    {
    }

    public EcommersContext(DbContextOptions<EcommersContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AttributeValues> AttributeValues { get; set; }

    public virtual DbSet<Banners> Banners { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Configuraciones> Configuraciones { get; set; }

    public virtual DbSet<ErrorLog> ErrorLog { get; set; }

    public virtual DbSet<MasterAttributes> MasterAttributes { get; set; }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<ProductVariantAttributes> ProductVariantAttributes { get; set; }

    public virtual DbSet<ProductVariantImages> ProductVariantImages { get; set; }

    public virtual DbSet<ProductVariantPriceHistory> ProductVariantPriceHistory { get; set; }

    public virtual DbSet<ProductVariants> ProductVariants { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Servicios> Servicios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FVE26L4\\MSSQLSERVER01;Database=Ecommers;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AttributeValues>(entity =>
        {
            entity.HasIndex(e => e.AttributeId, "IX_AttributeValues_AttributeId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.ValueDecimal).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.ValueString)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ValueText).HasColumnType("text");

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeValues)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AttributeValues_MasterAttributes");
        });

        modelBuilder.Entity<Banners>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Banners__3214EC070F4E70EC");

            entity.Property(e => e.AltText).HasMaxLength(255);
            entity.Property(e => e.BotonEnlace).HasMaxLength(255);
            entity.Property(e => e.BotonTexto).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Seccion).HasMaxLength(255);
            entity.Property(e => e.Subtitulo).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0734E09EB7");

            entity.HasIndex(e => e.ParentId, "IX_Categories_ParentId");

            entity.HasIndex(e => e.Slug, "UQ_Categories_Slug").IsUnique();

            entity.Property(e => e.BgClass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription).HasColumnType("text");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<Configuraciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC07BE0C2C95");

            entity.Property(e => e.AbreviacionEmpresa).HasMaxLength(255);
            entity.Property(e => e.BannerPrincipal).HasMaxLength(255);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.ColorTemaNavegador).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.EmailContacto).HasMaxLength(255);
            entity.Property(e => e.Facebook).HasMaxLength(255);
            entity.Property(e => e.Favicon).HasMaxLength(255);
            entity.Property(e => e.Idioma).HasMaxLength(50);
            entity.Property(e => e.Instagram).HasMaxLength(255);
            entity.Property(e => e.Linkedin).HasMaxLength(255);
            entity.Property(e => e.Logo).HasMaxLength(255);
            entity.Property(e => e.MetaAutor).HasMaxLength(255);
            entity.Property(e => e.MetaCanonical).HasMaxLength(255);
            entity.Property(e => e.MetaCharset).HasMaxLength(50);
            entity.Property(e => e.MetaImagenPredeterminada).HasMaxLength(255);
            entity.Property(e => e.MetaLanguage).HasMaxLength(50);
            entity.Property(e => e.MetaRevisitAfter).HasMaxLength(50);
            entity.Property(e => e.MetaRobots).HasMaxLength(50);
            entity.Property(e => e.MetaTitulo).HasMaxLength(255);
            entity.Property(e => e.MetaViewport).HasMaxLength(255);
            entity.Property(e => e.Moneda).HasMaxLength(50);
            entity.Property(e => e.NombreAplicacion).HasMaxLength(255);
            entity.Property(e => e.NombreEmpresa).HasMaxLength(255);
            entity.Property(e => e.OgImagen).HasMaxLength(255);
            entity.Property(e => e.OgSitename).HasMaxLength(255);
            entity.Property(e => e.OgTipo).HasMaxLength(50);
            entity.Property(e => e.OgTitulo).HasMaxLength(255);
            entity.Property(e => e.OgUrl).HasMaxLength(255);
            entity.Property(e => e.Pais).HasMaxLength(100);
            entity.Property(e => e.SchemaTipo).HasMaxLength(50);
            entity.Property(e => e.SimboloMoneda).HasMaxLength(10);
            entity.Property(e => e.Slogan).HasMaxLength(255);
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.TelefonoContacto).HasMaxLength(50);
            entity.Property(e => e.Tiktok).HasMaxLength(255);
            entity.Property(e => e.Twitter).HasMaxLength(255);
            entity.Property(e => e.TwitterCard).HasMaxLength(50);
            entity.Property(e => e.TwitterCreator).HasMaxLength(255);
            entity.Property(e => e.TwitterImagen).HasMaxLength(255);
            entity.Property(e => e.TwitterSite).HasMaxLength(255);
            entity.Property(e => e.TwitterTitulo).HasMaxLength(255);
            entity.Property(e => e.UltimaModificacion).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Whatsapp).HasMaxLength(50);
            entity.Property(e => e.Youtube).HasMaxLength(255);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.CodigoErrorLog).HasName("PK__ErrorLog__D6EEEB418569D92C");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MasterAttributes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MasterAt__3214EC074CA9A3E6");

            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.InputType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC078522FD26");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.HasIndex(e => e.ProductVariantsId, "IX_OrderItems_ProductVariantsId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.ProductVariants).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductVariantsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products_ProductId");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0715D299CD");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__CAC5E743A847117F").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("CLP");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.DeliveredAt).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrderNumber).HasMaxLength(255);
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.ShippedAt).HasColumnType("datetime");
            entity.Property(e => e.ShippingAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users_UserId");
        });

        modelBuilder.Entity<ProductVariantAttributes>(entity =>
        {
            entity.HasIndex(e => e.ValueId, "IX_ProductVariantAttributes_ValueId");

            entity.HasIndex(e => e.VariantId, "IX_ProductVariantAttributes_VariantId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Value).WithMany(p => p.ProductVariantAttributes)
                .HasForeignKey(d => d.ValueId)
                .HasConstraintName("FK_ProductVariantAttributes_Value");

            entity.HasOne(d => d.Variant).WithMany(p => p.ProductVariantAttributes)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK_ProductVariantAttributes_Variant");
        });

        modelBuilder.Entity<ProductVariantImages>(entity =>
        {
            entity.HasIndex(e => e.VariantId, "IX_ProductVariantImages_VariantId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Variant).WithMany(p => p.ProductVariantImages)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK_ProductVariantImages_Variant");
        });

        modelBuilder.Entity<ProductVariantPriceHistory>(entity =>
        {
            entity.HasIndex(e => e.ProductVariantId, "IX_ProductVariantPriceHistory_ProductVariantId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductVariantPriceHistory)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK_ProductVariantPriceHistory_Variant");
        });

        modelBuilder.Entity<ProductVariants>(entity =>
        {
            entity.HasIndex(e => e.ProductId, "IX_ProductVariants_ProductId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            entity.Property(e => e.ManageStock).HasDefaultValue(false);
            entity.Property(e => e.SKU)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StockStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("in_stock");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductVariants_Products");
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasIndex(e => new { e.IsActive, e.IsFeatured }, "IX_Products_IsActive_IsFeatured");

            entity.HasIndex(e => e.Slug, "UQ__Products__BC7B5FB65F71F323").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription).HasColumnType("text");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<Servicios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC0734E09EB7");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

    }
}
