using API.DTOs.RequisitionDTOs;

namespace API.Interfaces
{
    public interface IRequisitionsRepository
    {
        void AddNewRequisition(Requisition requisition);
        void RemoveRequisition(Requisition requisition);
        Task<Requisition> GetRequisitionById(int id);
        Task<IEnumerable<RequisitionDetailsDto>> GetRequisitionsForPart(int partId);
        Task<Requisition> GetNotOrderedRequisitionForPart(int partId);
        Task<IEnumerable<Requisition>> GetRequisitionsFromDate(DateTime fromDate);
        Task<PagedList<RequisitionDetailsDto>> GetRequisitions(PaginationParams pageParams);
    }
}