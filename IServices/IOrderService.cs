using ChemiseLab.Dto.Orders;

namespace ChemiseLab.IServices
{
    public interface IOrderService
    {
        Task Create_New_OrderAsync(NewOrder_Dto orderDto);
    }
}
