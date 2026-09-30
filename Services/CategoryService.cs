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
    }
}
