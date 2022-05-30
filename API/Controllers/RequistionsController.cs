using API.DTOs.RequisitionDTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
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
            var oldReq = await _unitOfWork.RequisitionsRepository.GetNotOrderedRequisitionForPart(reqDto.PartId);
            if (oldReq != null) return Conflict($"A requistion has already been made for {oldReq.Part?.PartCode}. Try using a PUT request to ammend");

            var newReq = _mapper.Map<Requisition>(reqDto);

            var part = await _unitOfWork.PartsRepository.GetPartById(reqDto.PartId);
            if (part == null) return BadRequest("Part doesn't exist");

            var userId = User.GetUserId();
            newReq.UserId = userId;

            if (newReq.ForBuffer)
            {
                if (part.BufferValue <= 0) return BadRequest("Part doesn't have a buffer value");

                newReq.Quantity = part.BufferValue - newReq.StockRemaining;
            }

            if (newReq.StockRemaining > 0)
                _unitOfWork.StockRepository.AddStockEntry(new StockLevelEntry
                {
                    PartId = part.Id,
                    UserId = userId,
                    RemainingStock = newReq.StockRemaining
                });

            _unitOfWork.RequisitionsRepository.AddNewRequisition(newReq);

            if(await _unitOfWork.Complete()) return Ok(newReq);
            return BadRequest("Issue adding requisition");
        }

        [HttpGet("{reqId}")]
        public async Task<ActionResult<IEnumerable<RequisitionDetailsDto>>> GetRequisition(int reqId)
        {
            var req = await _unitOfWork.RequisitionsRepository.GetRequisitionById(reqId);
            if (req == null) return NotFound("Couldn't find a requisition by that Id");

            return Ok(req);
        }
    }
}