using API.DTOs.RequisitionDTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    //[Authorize]
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
            var user = await _unitOfWork.UsersRepository.GetUserById(userId);
            newReq.UserId = userId;

            if (newReq.ForBuffer)
            {
                if (part.BufferValue <= 0) return BadRequest("Part doesn't have a buffer value");
                if (reqDto.StockRemaining == null) return BadRequest("For buffered items you must provide a remaining stock value");

                newReq.Quantity = part.BufferValue - (float)reqDto.StockRemaining;
            }

            if (reqDto.StockRemaining != null)
                _unitOfWork.StockRepository.AddStockEntry(new StockLevelEntry
                {
                    PartId = part.Id,
                    UserId = userId,
                    RemainingStock = (float)reqDto.StockRemaining
                });

            _unitOfWork.RequisitionsRepository.AddNewRequisition(newReq);

            if(await _unitOfWork.Complete()) return Ok(_mapper.Map<RequisitionDetailsDto>(newReq));
            return BadRequest("Issue adding requisition");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRequisition([FromBody] NewRequisitionDto reqDto)
        {
            var oldReq = await _unitOfWork.RequisitionsRepository.GetNotOrderedRequisitionForPart(reqDto.PartId);
            if(oldReq == null) return NotFound("Cannot replace a none existant requisition");
            
            // Update urgency
            if (reqDto.Urgent) oldReq.Urgent = true;
            oldReq.Quantity = reqDto.Quantity;

            // Update user
            var user = await _unitOfWork.UsersRepository.GetUserById(User.GetUserId());
            oldReq.User = user;

            // Add new stock entry
            if (reqDto.StockRemaining != null)
                _unitOfWork.StockRepository.AddStockEntry(new StockLevelEntry
                {
                    PartId = oldReq.PartId,
                    UserId = user.Id,
                    RemainingStock = (float)reqDto.StockRemaining
                });

            // Update quantity
            oldReq.Quantity = reqDto.Quantity;

            // Update date
            oldReq.Date = DateTime.Now;

            if(await _unitOfWork.Complete()) return Ok(_mapper.Map<RequisitionDetailsDto>(oldReq));
            return BadRequest("Issue updating requisition");
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