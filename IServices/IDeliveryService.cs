using ChemiseLab.Dto.Delivery;
using ChemiseLab.Models;

namespace ChemiseLab.IServices
{
    public interface IDeliveryService
    {
        Task<string> CreateParcelAsync(Order order, string products);
        Task<List<DistrictsList_Dto>> GetDistrictsAsync();
    }
}
