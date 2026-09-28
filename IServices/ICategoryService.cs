using ChemiseLab.Dto.Categories;
using ChemiseLab.Models;

namespace ChemiseLab.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategories_Dto>> GetAllCategoriesAsync();
        Task<IEnumerable<Get_S_Categories_Dto>> GetAll_Sub_Categories_ByIdCat_Async(int IdCat);
        Task<string?> GetCategoryNameByIdAsync(int idCategorie);
        Task<string?> GetSubCategoryNameByIdAsync(int idSubCategorie);
    }
}
