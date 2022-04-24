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
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers([FromQuery]PaginationParams supplierParams, [FromQuery]string searchValue)
        {
            Func<Supplier, bool> predicate;
            if (searchValue == null) predicate = x => true;
            else predicate = x => (x.NormalizedName.Contains(searchValue.ToUpper())
                                || (x.Website == null ? false : x.Website.ToUpper().Contains(searchValue.ToUpper())));

            var suppliers = await _unitOfWork.SuppliersRepository.GetSuppliers(supplierParams, predicate);
            Response.AddPaginationHeader(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);

            return Ok(suppliers);
        }

        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<String>>> GetSupplierNames()
        {
            var names = await _unitOfWork.SuppliersRepository.GetAllSupplierNames();
            if (names == null) NotFound("No suppliers found");

            return Ok(names);
        }

        [HttpGet("{supplierName}")]
        public async Task<ActionResult<Supplier>> GetSupplier(string supplierName)
        {
            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(supplierName);
            if (supplier == null) return NotFound("Couldn't find a supplier called " + supplierName);

            return Ok(supplier);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateSupplier([FromBody] NewSupplierDto supplier)
        {
            if (await _unitOfWork.SuppliersRepository.Exists(supplier.Name)) return BadRequest("That supplier already exists");

            _unitOfWork.SuppliersRepository.AddSupplier(supplier);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem creating supplier");
        }
        
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteSupplier([FromQuery]string supplierName)
        {
            await _unitOfWork.SuppliersRepository.RemoveSupplierByName(supplierName);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue deleting supplier");
        }

        [HttpPut("{supplierName}")]
        public async Task<ActionResult> MergeSuppliers(string supplierName, [FromBody] IEnumerable<string> suppliers)
        {
            var mainSupplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(supplierName);
            if (mainSupplier == null) return BadRequest("No supplier found by that name");
            if (suppliers == null) return BadRequest("You must provide suppliers to merge");

            List<Supplier> suppliersToMerge = new List<Supplier>();
            foreach(var name in suppliers)
                suppliersToMerge.Add(await _unitOfWork.SuppliersRepository.GetSupplierByName(name));

            

            await _unitOfWork.Complete();
            return Ok();
        }
    }
}