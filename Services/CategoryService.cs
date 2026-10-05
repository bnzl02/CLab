using ChemiseLab.Data;
using ChemiseLab.Dto.Categories;
using ChemiseLab.IServices;
using ChemiseLab.Models;
using Microsoft.EntityFrameworkCore;

namespace ChemiseLab.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _contextDB;

        public CategoryService(ApplicationDbContext context)
        {
            _contextDB = context;
        }

        public async Task<IEnumerable<GetCategories_Dto>> GetAllCategoriesAsync()
        {
            return await _contextDB.Categories
                .AsNoTracking()
                .Select(c => new GetCategories_Dto
                {
                    Category_ID = c.IdCategorie,
                    Category_Name = c.Libelle
                })
                .ToListAsync();
        }

        public async Task<string?> GetCategoryNameByIdAsync(int idCategorie)
        {
            return await _contextDB.Categories
                .Where(c => c.IdCategorie == idCategorie)
                .Select(c => c.Libelle)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetSubCategoryNameByIdAsync(int idSubCategorie)
        {
            return await _contextDB.SousCategories
                .Where(c => c.IdSousCategorie == idSubCategorie)
                .Select(c => c.Libelle)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Get_S_Categories_Dto>> GetAll_Sub_Categories_ByIdCat_Async(int IdCat)
        {
            return await _contextDB.SousCategories
                .AsNoTracking()
                .Where(sc => sc.IdCategorie == IdCat)
                .Select(c => new Get_S_Categories_Dto
                {
                    S_Category_ID = c.IdSousCategorie,
                    S_Category_Name = c.Libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetCategories_Dto>> GetAllCategoriesExceptAsync(int idCategorie)
        {
            return await _contextDB.Categories
                .AsNoTracking()
                .Where(c => c.IdCategorie != idCategorie)
                .Select(c => new GetCategories_Dto
                {
                    Category_ID = c.IdCategorie,
                    Category_Name = c.Libelle
                })
                .ToListAsync();
        }

        public async Task<int> CreateCategoryAsync(AddCategory_Dto categoryDto)
        {
            var existeDeja = await _contextDB.Categories
                .AnyAsync(c => c.Libelle == categoryDto.Libelle);

            if (existeDeja)
                throw new ArgumentException($"Une catégorie avec le libellé '{categoryDto.Libelle}' existe déjà.");

            var categorie = new Categorie
            {
                Libelle = categoryDto.Libelle
            };

            _contextDB.Categories.Add(categorie);
            await _contextDB.SaveChangesAsync();

            return categorie.IdCategorie;
        }

        public async Task<int> CreateSousCategoryAsync(Add_SubCategory_Dto sousCategoryDto)
        {
            var categorieExiste = await _contextDB.Categories
                .AnyAsync(c => c.IdCategorie == sousCategoryDto.IdCategorie);

            if (!categorieExiste)
                throw new ArgumentException($"Catégorie parente introuvable : {sousCategoryDto.IdCategorie} .Merci de créer la catégorie premierement !!");

            var existeDeja = await _contextDB.SousCategories
                .AnyAsync(sc => sc.Libelle == sousCategoryDto.Libelle && sc.IdCategorie == sousCategoryDto.IdCategorie);

            if (existeDeja)
                throw new ArgumentException($"Une sous-catégorie avec le libellé '{sousCategoryDto.Libelle}' existe déjà pour cette catégorie.");

            var sousCategorie = new SousCategorie
            {
                Libelle = sousCategoryDto.Libelle,
                IdCategorie = sousCategoryDto.IdCategorie
            };

            _contextDB.SousCategories.Add(sousCategorie);
            await _contextDB.SaveChangesAsync();

            return sousCategorie.IdSousCategorie;
        }
    }
}
