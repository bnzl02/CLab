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
            return await _contextDB.Produits
        .AsNoTracking()
        .Where(p => p.IdSousCategorie == idSubCat)
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
        public async Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdCat_Async(int idCat)
        {
            List<int> _ListSubCat = _contextDB.SousCategories.Where(sc=> sc.IdCategorie == idCat).Select(sc=>sc.IdSousCategorie).ToList();

            return await _contextDB.Produits
        .AsNoTracking()
        .Where(p => _ListSubCat.Contains(p.IdSousCategorie))
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

        public async Task<Get_DetailsProduct_Dto> Get_DetailsProduct_ByIdAsync(int IdP)
        {
            var produit = await _contextDB.Produits
                        .AsNoTracking()
                        .Where(c => c.IdProduit == IdP)
                        .Select(c => new Get_DetailsProduct_Dto
                        {
                            Product_ID = c.IdProduit,
                            Product_Name = c.Libelle,
                            Product_Price = c.Prix,
                            Product_Ref = c.Reference,
                            Product_Desc = c.Description,
                        })
                        .FirstOrDefaultAsync();

            if (produit == null)
                return null;

            // Récupérer images, couleurs, tailles séparément (une requête chacune, réutilisation des méthodes existantes)
            produit.Product_Images = (await Get_AllImages_ByIdProduct(IdP)).ToList();
            produit.Product_Colors = (await Get_AllColorsProduct_ByIdProduct_Async(IdP)).ToList();
            //produit.Product_Sizes = (await Get_AllSizesProduct_ByIdProduct_Async(IdP)).ToList();

            return produit;
        }

        public async Task<List<ProduitImages_Dto>> Get_AllImages_ByIdProduct(int idProduct)
        {
            return await _contextDB.Images
                .AsNoTracking()
                .Where(i => i.IdProduit == idProduct)
                .Select(i => new ProduitImages_Dto
                {
                    Id = i.IdImage,
                    Url = i.UrlImage,
                }).ToListAsync();
        }

        public async Task<List<Get_ColorsProduct_Dto>> Get_AllColorsProduct_ByIdProduct_Async(int idProduct)
        {
            // 1. Récupérer toutes les combinaisons (couleur + taille) pour ce produit, en une seule requête SQL
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
                        sc.c.LibelleCouleur,
                        t.IdTaille,
                        t.LibelleFr,
                        t.LibelleSport,
                        t.LibelleUsa
                    })
                .ToListAsync();

            // 2. Regrouper par couleur (en mémoire)
            var result = stocks
                .GroupBy(s => new { s.IdCouleur, s.LibelleCouleur })
                .Select(g => new Get_ColorsProduct_Dto
                {
                    ID = g.Key.IdCouleur,
                    Libelle = g.Key.LibelleCouleur,
                    Sizes = g.Select(x => new Get_SizesProduct_Dto
                    {
                        ID = x.IdTaille,
                        Size_Classy = x.LibelleFr,
                        Size_Sport = x.LibelleSport,
                        Size_Pants_USA = x.LibelleUsa
                    })
                    .DistinctBy(t => t.ID)
                    .ToList()
                })
                .ToList();

            return result;
        }
    }
}
