using ChemiseLab.Dto.Products;

namespace ChemiseLab.IServices
{
    public interface IProductsService
    {
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdCat_Async(int idCat);
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdSubCat_Async(int idSubCat);
        Task<Get_DetailsProduct_Dto> Get_DetailsProduct_ByIdAsync(int idProduct, int idColor);
        Task<IEnumerable<Get_AllProducts_Suggestion_Dto>> GetAllProductsAsync();
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByListIdCat_Async(List<int> idSubCat);
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ExceptIdCat_Async(int idCat);
        Task<int> CreateProductAsync(AddProduct_Dto productDto);
    }
}
