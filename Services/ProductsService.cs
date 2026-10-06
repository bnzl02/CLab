using ChemiseLab.Data;
using ChemiseLab.Dto.Categories;
using ChemiseLab.Dto.Colors;
using ChemiseLab.Dto.Products;
using ChemiseLab.Dto.Sizes;
using ChemiseLab.IServices;
using ChemiseLab.Models;
using Microsoft.EntityFrameworkCore;

namespace ChemiseLab.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _contextDB;

        public ProductsService(ApplicationDbContext context)
        {
            _contextDB = context;
        }

        public async Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdSubCat_Async(int idSubCat)
        {
            // 1. Récupérer les produits actifs de cette sous-catégorie
            var produits = await _contextDB.Produits
                .AsNoTracking()
                .Where(p => p.IdSousCategorie == idSubCat && p.Actif != "NAC")
                .ToListAsync();

            var idsProduits = produits.Select(p => p.IdProduit).ToList();

            // 2. Récupérer toutes les combinaisons produit+couleur disponibles (via Stocks)
            var productCouleurs = await _contextDB.Stocks
                .Where(s => idsProduits.Contains(s.IdProduit))
                .Select(s => new { s.IdProduit, s.IdCouleur })
                .Distinct()
                .ToListAsync();

            // 3. Récupérer toutes les images concernées, avec leur couleur
            var images = await _contextDB.Images
                .Where(img => idsProduits.Contains(img.IdProduit))
                .Select(img => new { img.IdProduit, img.IdCouleur, img.IdImage, img.UrlImage })
                .ToListAsync();

            // 4. Assembler : une entrée par (produit, couleur)
            var result = new List<Get_Products_Dto>();

            foreach (var p in produits)
            {
                var couleursDuProduit = productCouleurs.Where(pc => pc.IdProduit == p.IdProduit);

                foreach (var couleur in couleursDuProduit)
                {
                    result.Add(new Get_Products_Dto
                    {
                        Product_ID = p.IdProduit,
                        Product_Name = p.Libelle,
                        Product_Price = p.Prix,
                        Product_Couleur_ID = couleur.IdCouleur,
                        Product_Status = p.Actif,
                        Product_Images = images
                            .Where(img => img.IdProduit == p.IdProduit && img.IdCouleur == couleur.IdCouleur)
                            .Select(img => new ProduitImages_Dto
                            {
                                Id = img.IdImage,
                                Url = img.UrlImage
                            })
                            .ToList()
                    });
                }
            }

            return result;
        }

        public async Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdCat_Async(int idCat)
        {
            List<int> _ListSubCat = _contextDB.SousCategories.Where(sc=> sc.IdCategorie == idCat).Select(sc=>sc.IdSousCategorie).ToList();
            
            // 1. Récupérer les produits actifs de cette sous-catégorie
            var produits = await _contextDB.Produits
                .AsNoTracking()
                .Where(p => _ListSubCat.Contains(p.IdSousCategorie) && p.Actif != "NAC")
                .ToListAsync();

            var idsProduits = produits.Select(p => p.IdProduit).ToList();

            // 2. Récupérer toutes les combinaisons produit+couleur disponibles (via Stocks)
            var productCouleurs = await _contextDB.Stocks
                .Where(s => idsProduits.Contains(s.IdProduit))
                .Select(s => new { s.IdProduit, s.IdCouleur })
                .Distinct()
                .ToListAsync();

            // 3. Récupérer toutes les images concernées, avec leur couleur
            var images = await _contextDB.Images
                .Where(img => idsProduits.Contains(img.IdProduit))
                .Select(img => new { img.IdProduit, img.IdCouleur, img.IdImage, img.UrlImage })
                .ToListAsync();

            // 4. Assembler : une entrée par (produit, couleur)
            var result = new List<Get_Products_Dto>();

            foreach (var p in produits)
            {
                var couleursDuProduit = productCouleurs.Where(pc => pc.IdProduit == p.IdProduit);

                foreach (var couleur in couleursDuProduit)
                {
                    result.Add(new Get_Products_Dto
                    {
                        Product_ID = p.IdProduit,
                        Product_Name = p.Libelle,
                        Product_Price = p.Prix,
                        Product_Couleur_ID = couleur.IdCouleur,
                        Product_Status = p.Actif,
                        Product_Images = images
                            .Where(img => img.IdProduit == p.IdProduit && img.IdCouleur == couleur.IdCouleur)
                            .Select(img => new ProduitImages_Dto
                            {
                                Id = img.IdImage,
                                Url = img.UrlImage
                            })
                            .ToList()
                    });
                }
            }

            return result;
        }

        public async Task<Get_DetailsProduct_Dto> Get_DetailsProduct_ByIdAsync(int IdP, int idClr)
        {
            var produit = await _contextDB.Produits
                        .AsNoTracking()
                        .Where(c => c.IdProduit == IdP && c.Actif == "AC")
                        .Select(c => new Get_DetailsProduct_Dto
                        {
                            Product_ID = c.IdProduit,
                            Product_Category = c.IdSousCategorieNavigation.IdCategorieNavigation.IdCategorie,
                            Product_Name = c.Libelle,
                            Product_Price = c.Prix,
                            Product_Ref = c.Reference,
                            Product_Desc = c.Description,
                            
                        })
                        .FirstOrDefaultAsync();

            if (produit == null)
                return null;

            // Récupérer images, couleurs, tailles séparément (une requête chacune, réutilisation des méthodes existantes)
            produit.Product_Images = (await Get_AllImages_ByIdProduct(IdP,idClr)).ToList();
            produit.Product_Colors = (await Get_AllColorsProduct_ByIdProduct_Async(IdP, produit.Product_Category)).ToList();
            //produit.Product_Sizes = (await Get_AllSizesProduct_ByIdProduct_Async(IdP)).ToList();

            return produit;
        }

        public async Task<List<ProduitImages_Dto>> Get_AllImages_ByIdProduct(int idProduct, int idColor)
        {
            return await _contextDB.Images
                .AsNoTracking()
                .Where(i => i.IdProduit == idProduct && i.IdCouleur == idColor)
                .Select(i => new ProduitImages_Dto
                {
                    Id = i.IdImage,
                    Url = i.UrlImage,

                }).ToListAsync();
        }

        public async Task<List<Get_ColorsProduct_Dto>> Get_AllColorsProduct_ByIdProduct_Async(int idProduct, int category)
        {
            // 1. Récupérer toutes les combinaisons (couleur + taille) pour ce produit, en une seule requête
            var stocks = await _contextDB.Stocks
                .AsNoTracking()
                .Where(s => s.IdProduit == idProduct)
                .Join(_contextDB.Couleurs,
                    s => s.IdCouleur,
                    c => c.IdCouleur,
                    (s, c) => new { s, c })
                .Join(_contextDB.Tailles,
                    sc => sc.s.IdTaille,
                    t => t.IdTaille,
                    (sc, t) => new
                    {
                        sc.c.IdCouleur,
                        sc.c.HexaCouleur,
                        t.IdTaille,
                        t.LibelleFr,
                        t.LibelleSport,
                        t.LibelleUsa
                    })
                .ToListAsync();

            // 2. Regrouper par couleur (en mémoire)
            var result = stocks
                .GroupBy(s => s.IdCouleur)
                .Select(g => new Get_ColorsProduct_Dto
                {
                    ID = g.Key,
                    HexaCode = g.First().HexaCouleur ?? new List<string>(),
                    Sizes = g.Select(x => new Get_SizesProduct_Dto
                    {
                        ID = x.IdTaille,
                        Size_Classy = x.LibelleFr,
                        Size_Sport = x.LibelleSport,
                        Size_Pants_USA = category == 2 ? x.LibelleUsa : null,
                    })
                    .DistinctBy(t => t.ID)
                    .ToList()
                })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<Get_AllProducts_Suggestion_Dto>> GetAllProductsAsync()
        {
            return await _contextDB.Produits
                .AsNoTracking()
                .Where(p=> p.Actif != "NAC")
                .Select(c => new Get_AllProducts_Suggestion_Dto
                {
                    Product_ID = c.IdProduit,
                    Product_Name = c.Libelle,
                    Product_Price = c.Prix,
                    Product_Ref = c.Reference,
                    Product_Catg = c.IdSousCategorieNavigation.IdCategorieNavigation.Libelle,
                    Product_S_Catg = c.IdSousCategorieNavigation.IdCategorieNavigation.Libelle,
                    Product_Images = c.Images.Select(img => new ProduitImages_Dto
                    {
                        Id = img.IdImage,
                        Url = img.UrlImage
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByListIdCat_Async(List<int> _L_idCat)
        {
            return await _contextDB.Produits
            .AsNoTracking()
            .Where(p => _L_idCat.Contains(p.IdSousCategorieNavigation.IdCategorie) && p.Actif != "NAC")
            .GroupJoin(
                _contextDB.Images,
                p => p.IdProduit,
                img => img.IdProduit,
                (p, images) => new Get_Products_Dto
                {
                    Product_ID = p.IdProduit,
                    Product_Name = p.Libelle,
                    Product_Price = p.Prix,
                    Product_Images = images.Select(img => new ProduitImages_Dto
                    {
                        Id = img.IdImage,
                        Url = img.UrlImage

                    }).ToList()
                })
            .ToListAsync();
        }

        public async Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ExceptIdCat_Async(int idCat)
        {
            return await _contextDB.Produits
                .AsNoTracking()
                .Where(p => p.IdSousCategorieNavigation.IdCategorie != idCat && p.Actif != "NAC")
                .Select(p => new Get_Products_Dto
                {
                    Product_ID = p.IdProduit,
                    Product_Name = p.Libelle,
                    Product_Price = p.Prix,
                    Product_Images = p.Images.Select(img => new ProduitImages_Dto
                    {
                        Id = img.IdImage,
                        Url = img.UrlImage

                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<int> CreateProductAsync(AddProduct_Dto productDto)
        {
            var sousCategorieExiste = await _contextDB.SousCategories
                .AnyAsync(sc => sc.IdSousCategorie == productDto.IdSousCategorie);

            if (!sousCategorieExiste)
                throw new ArgumentException($"Sous-catégorie introuvable : {productDto.IdSousCategorie}");

            // Vérifier que la référence n'existe pas déjà (unicité métier)
            var referenceExiste = await _contextDB.Produits
                .AnyAsync(p => p.Reference == productDto.Product_Reference);

            if (referenceExiste)
                throw new ArgumentException($"Un produit avec la référence '{productDto.Product_Reference}' existe déjà.");

            var produit = new Produit
            {
                Libelle = productDto.Product_Libelle,
                Description = productDto.Product_Description,
                Prix = productDto.Product_Prix,
                Reference = productDto.Product_Reference,
                IdSousCategorie = productDto.IdSousCategorie,
                Actif = "AC",
            };

            _contextDB.Produits.Add(produit);
            await _contextDB.SaveChangesAsync();

            return produit.IdProduit;
        }
    }
}
