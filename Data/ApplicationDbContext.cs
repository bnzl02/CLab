using System;
using System.Collections.Generic;
using ChemiseLab.Models;
using Microsoft.EntityFrameworkCore;

namespace ChemiseLab.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Couleur> Couleurs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<LigneOrder> LigneOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<SousCategorie> SousCategories { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Taille> Tailles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.IdCategorie).HasName("categorie_pkey");

            entity.ToTable("categorie");

            entity.Property(e => e.IdCategorie).HasColumnName("id_categorie");
            entity.Property(e => e.Libelle)
                .HasMaxLength(100)
                .HasColumnName("libelle");
        });

        modelBuilder.Entity<Couleur>(entity =>
        {
            entity.HasKey(e => e.IdCouleur).HasName("couleur_pkey");

            entity.ToTable("couleur");

            entity.Property(e => e.IdCouleur).HasColumnName("id_couleur");
            entity.Property(e => e.LibelleCouleur)
                .HasMaxLength(50)
                .HasColumnName("libelle_couleur");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("image_pkey");

            entity.ToTable("image");

            entity.HasIndex(e => e.IdProduit, "idx_image_produit");

            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.IdProduit).HasColumnName("id_produit");
            entity.Property(e => e.UrlImage)
                .HasMaxLength(255)
                .HasColumnName("url_image");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("image_id_produit_fkey");
        });

        modelBuilder.Entity<LigneOrder>(entity =>
        {
            entity.HasKey(e => e.IdLigneOrder).HasName("ligne_order_pkey");

            entity.ToTable("ligne_order");

            entity.HasIndex(e => e.IdCouleur, "idx_ligne_order_couleur");

            entity.HasIndex(e => e.IdOrder, "idx_ligne_order_order");

            entity.HasIndex(e => e.IdProduit, "idx_ligne_order_produit");

            entity.HasIndex(e => e.IdTaille, "idx_ligne_order_taille");

            entity.Property(e => e.IdLigneOrder).HasColumnName("id_ligne_order");
            entity.Property(e => e.IdCouleur).HasColumnName("id_couleur");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduit).HasColumnName("id_produit");
            entity.Property(e => e.IdTaille).HasColumnName("id_taille");
            entity.Property(e => e.PrixUnitaire)
                .HasPrecision(10, 2)
                .HasColumnName("prix_unitaire");
            entity.Property(e => e.Quantite).HasColumnName("quantite");

            entity.HasOne(d => d.IdCouleurNavigation).WithMany(p => p.LigneOrders)
                .HasForeignKey(d => d.IdCouleur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ligne_order_id_couleur_fkey");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.LigneOrders)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ligne_order_id_order_fkey");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.LigneOrders)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ligne_order_id_produit_fkey");

            entity.HasOne(d => d.IdTailleNavigation).WithMany(p => p.LigneOrders)
                .HasForeignKey(d => d.IdTaille)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ligne_order_id_taille_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.AdresseClient)
                .HasMaxLength(255)
                .HasColumnName("adresse_client");
            entity.Property(e => e.DateOrder)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_order");
            entity.Property(e => e.NomClient)
                .HasMaxLength(100)
                .HasColumnName("nom_client");
            entity.Property(e => e.PaysClient)
                .HasMaxLength(100)
                .HasColumnName("pays_client");
            entity.Property(e => e.PrenomClient)
                .HasMaxLength(100)
                .HasColumnName("prenom_client");
            entity.Property(e => e.ReferenceLivraison)
                .HasMaxLength(50)
                .HasColumnName("reference_livraison");
            entity.Property(e => e.StatutOrder)
                .HasMaxLength(50)
                .HasColumnName("statut_order");
            entity.Property(e => e.TelephoneClient)
                .HasMaxLength(20)
                .HasColumnName("telephone_client");
            entity.Property(e => e.TotalOrder)
                .HasPrecision(10, 2)
                .HasColumnName("total_order");
            entity.Property(e => e.VilleClient)
                .HasMaxLength(100)
                .HasColumnName("ville_client");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.IdProduit).HasName("produits_pkey");

            entity.ToTable("produits");

            entity.HasIndex(e => e.IdSousCategorie, "idx_produit_sous_categorie");

            entity.Property(e => e.IdProduit).HasColumnName("id_produit");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdSousCategorie).HasColumnName("id_sous_categorie");
            entity.Property(e => e.Libelle)
                .HasMaxLength(150)
                .HasColumnName("libelle");
            entity.Property(e => e.Prix)
                .HasPrecision(10, 2)
                .HasColumnName("prix");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .HasColumnName("reference");

            entity.HasOne(d => d.IdSousCategorieNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.IdSousCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produits_id_sous_categorie_fkey");
        });

        modelBuilder.Entity<SousCategorie>(entity =>
        {
            entity.HasKey(e => e.IdSousCategorie).HasName("sous_categorie_pkey");

            entity.ToTable("sous_categorie");

            entity.HasIndex(e => e.IdCategorie, "idx_sous_categorie_categorie");

            entity.Property(e => e.IdSousCategorie).HasColumnName("id_sous_categorie");
            entity.Property(e => e.IdCategorie).HasColumnName("id_categorie");
            entity.Property(e => e.Libelle)
                .HasMaxLength(100)
                .HasColumnName("libelle");

            entity.HasOne(d => d.IdCategorieNavigation).WithMany(p => p.SousCategories)
                .HasForeignKey(d => d.IdCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sous_categorie_id_categorie_fkey");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.IdProduit, e.IdTaille, e.IdCouleur }).HasName("stock_pkey");

            entity.ToTable("stock");

            entity.HasIndex(e => e.IdCouleur, "idx_stock_couleur");

            entity.HasIndex(e => e.IdProduit, "idx_stock_produit");

            entity.HasIndex(e => e.IdTaille, "idx_stock_taille");

            entity.Property(e => e.IdProduit).HasColumnName("id_produit");
            entity.Property(e => e.IdTaille).HasColumnName("id_taille");
            entity.Property(e => e.IdCouleur).HasColumnName("id_couleur");
            entity.Property(e => e.Stock1).HasColumnName("stock");

            entity.HasOne(d => d.IdCouleurNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdCouleur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_id_couleur_fkey");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_id_produit_fkey");

            entity.HasOne(d => d.IdTailleNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdTaille)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_id_taille_fkey");
        });

        modelBuilder.Entity<Taille>(entity =>
        {
            entity.HasKey(e => e.IdTaille).HasName("taille_pkey");

            entity.ToTable("taille");

            entity.Property(e => e.IdTaille).HasColumnName("id_taille");
            entity.Property(e => e.LibelleFr)
                .HasMaxLength(50)
                .HasColumnName("libelle_fr");
            entity.Property(e => e.LibelleSport)
                .HasMaxLength(50)
                .HasColumnName("libelle_sport");
            entity.Property(e => e.LibelleUsa)
                .HasMaxLength(50)
                .HasColumnName("libelle_usa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
