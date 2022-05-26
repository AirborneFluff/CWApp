namespace API.Interfaces
{
    public interface IRequisitionsRepository
    {
        void AddNewRequisition(Requisition requisition);
        void RemoveRequisition(Requisition requisition);
        Task<Requisition> GetRequisitionById(int id);
        Task<IEnumerable<Requisition>> GetRequisitionsForPart(int partId);
        Task<IEnumerable<Requisition>> GetRequisitionsFromDate(DateTime fromDate);
        Task<PagedList<Requisition>> GetRequisitions(PaginationParams pageParams);
    }
}