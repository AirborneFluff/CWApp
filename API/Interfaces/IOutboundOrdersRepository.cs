namespace API.Interfaces
{
    public interface IOutboundOrdersRepository
    {
        void AddOrder(OutboundOrder order);
        void RemoveOrder(OutboundOrder order);
        Task<OutboundOrder> GetOutboundOrderById(int id);
        Task<OutboundOrder> GetOutboundOrderByOrderNumber(int orderNumber);
        Task<OutboundOrder> GetOutboundOrderBySupplierRef(string supplierRef);
    }
}