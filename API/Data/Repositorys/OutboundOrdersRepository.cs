namespace API.Data.Repositorys
{
    public class OutboundOrdersRepository : IOutboundOrdersRepository
    {
        private readonly DataContext _context;
        public OutboundOrdersRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddOrder(OutboundOrder order)
        {
            _context.OutboundOrders.Add(order);
        }

        public Task<OutboundOrder> GetOutboundOrderById(int id)
        {
            return _context.OutboundOrders
                .Include(o => o.Items)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<OutboundOrder> GetOutboundOrderByOrderNumber(int orderNumber)
        {
            return _context.OutboundOrders
                .Include(o => o.Items)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public Task<OutboundOrder> GetOutboundOrderBySupplierRef(string supplierRef)
        {
            return _context.OutboundOrders
                .Include(o => o.Items)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.SupplierReference == supplierRef);
        }

        public void RemoveOrder(OutboundOrder order)
        {
            _context.OutboundOrders.Remove(order);
        }
    }
}