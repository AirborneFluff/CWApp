using System.Runtime.Intrinsics.X86;
using API.DTOs.RequisitionDTOs;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Parts
            CreateMap<Part, PartDto>();
            CreateMap<NewPartDto, Part>();
            CreateMap<UpdateSupplySourceDto, SupplySource>();

            // Requisitions
            CreateMap<NewRequisitionDto, Requisition>();
            CreateMap<Requisition, RequisitionDetailsDto>();

            CreateMap<Part, Requisition_PartDto>();
            CreateMap<AppUser, Requisition_UserDto>();
            CreateMap<OutboundOrder, Requisition_OutboundOrderDto>();
        }
    }
}