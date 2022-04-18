using API.Helpers;

namespace API.Controllers
{
    public class SuppliersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public SuppliersController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers([FromQuery]PaginationParams supplierParams)
        {
            var suppliers = await _unitOfWork.SuppliersRepository.GetSuppliers(supplierParams);
            Response.AddPaginationHeader(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);

            return Ok(suppliers);
        }

        [HttpGet("{supplierName}")]
        public async Task<ActionResult<Supplier>> GetSupplier(string supplierName)
        {
            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(supplierName);
            if (supplier == null) return NotFound("Couldn't find a supplier called " + supplierName);

            return Ok(supplier);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateSupplier([FromBody] Supplier supplier)
        {
            if (await _unitOfWork.SuppliersRepository.Exists(supplier.Name)) return BadRequest("That supplier already exists");

            _unitOfWork.SuppliersRepository.AddSupplier(supplier);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem creating supplier");
        }
        
        [HttpDelete("{supplierName}")]
        public async Task<ActionResult> DeleteSupplier(string supplierName)
        {
            await _unitOfWork.SuppliersRepository.RemoveSupplierByName(supplierName);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue deleting supplier");
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            await _unitOfWork.SuppliersRepository.RemoveSupplierById(id);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue deleting supplier");
        }
    }
}