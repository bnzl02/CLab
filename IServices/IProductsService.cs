using ChemiseLab.Dto.Products;

namespace ChemiseLab.IServices
{
    public interface IProductsService
    {
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdCat_Async(int idCat);
        Task<IEnumerable<Get_Products_Dto>> Get_AllProducts_ByIdSubCat_Async(int idSubCat);
        Task<Get_DetailsProduct_Dto> Get_DetailsProduct_ByIdAsync(int idProduct);
    }
}
