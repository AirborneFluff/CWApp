using API.DTOs.RequisitionDTOs;

namespace API.Interfaces
{
    public interface IRequisitionsRepository
    {
        void AddNewRequisition(Requisition requisition);
        void RemoveRequisition(Requisition requisition);
        Task<RequisitionDetailsDto> GetRequisitionById(int id);
        Task<IEnumerable<RequisitionDetailsDto>> GetRequisitionsForPart(int partId);
        Task<RequisitionDetailsDto> GetNotOrderedRequisitionForPart(int partId);
        Task<IEnumerable<Requisition>> GetRequisitionsFromDate(DateTime fromDate);
        Task<PagedList<RequisitionDetailsDto>> GetRequisitions(PaginationParams pageParams);
    }
}