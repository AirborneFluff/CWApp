using API.DTOs.RequisitionDTOs;

namespace API.Controllers
{
    public class RequisitionsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RequisitionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Requisition>>> GetRequisitions([FromQuery] PaginationParams pageParams)
        {
            var requistions = await _unitOfWork.RequisitionsRepository.GetRequisitions(pageParams);
            Response.AddPaginationHeader(requistions.CurrentPage, requistions.PageSize, requistions.TotalCount, requistions.TotalPages);

            return Ok(requistions);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRequisiton([FromBody] NewRequisitionDto reqDto)
        {
            var req = await _unitOfWork.RequisitionsRepository.GetNotOrderedRequisitionForPart(reqDto.PartId);
            if (req != null) return Conflict($"A requistion has already been made for {req.Part?.PartCode}. Try using a PUT request to ammend");

            req = _mapper.Map<Requisition>(reqDto);
            _unitOfWork.RequisitionsRepository.AddNewRequisition(req);

            if(await _unitOfWork.Complete()) return Ok(req);
            return BadRequest("Issue adding requisition");
        }
    }
}