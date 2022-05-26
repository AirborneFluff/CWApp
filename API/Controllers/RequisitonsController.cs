namespace API.Controllers
{
    public class RequisitonsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequisitonsController(IUnitOfWork unitOfWork)
        {
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
        public async Task<ActionResult> CreateRequisiton()
        {
            return Ok();
        }
    }
}