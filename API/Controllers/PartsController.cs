using System.Collections.ObjectModel;
using System.Security.Claims;
using API.Helpers;

namespace API.Controllers
{
    public class PartsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PartsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartDto>>> GetParts([FromQuery] PaginationParams partParams, [FromQuery] string searchValue)
        {
            Func<PartDto, bool> predicate;
            if (searchValue == null) predicate = x => true;
            else predicate = x => (x.PartCode.Contains(searchValue)
                                || (x.Description == null ? false : x.Description.ToUpper().Contains(searchValue.ToUpper())));

            var parts = await _unitOfWork.PartsRepository.GetParts(partParams, predicate);
            Response.AddPaginationHeader(parts.CurrentPage, parts.PageSize, parts.TotalCount, parts.TotalPages);

            return Ok(parts);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PartDto>>> GetAllParts()
        {
            var parts = await _unitOfWork.PartsRepository.GetAllParts();
            return Ok(parts);
        }
        

        [HttpGet("{partcode}")]
        public async Task<ActionResult<Part>> GetPart(string partCode)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            return Ok(part);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreatePart([FromBody] NewPartDto part)
        {
            if (await _unitOfWork.PartsRepository.Exists(part.PartCode)) return BadRequest("A part already exists with that partcode");

            _unitOfWork.PartsRepository.AddPart(part);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem creating part");
        }

        [HttpPatch("{partCode}/add-source")]
        public async Task<ActionResult> AddSupplySource (string partCode, [FromBody] SupplySourceDto sourceDto)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(sourceDto.SupplierName);
            if (supplier == null) return NotFound("Couldn't find a supplier with that name. Create one first");

            var supplySource = new SupplySource
            {
                Supplier = supplier,
                SupplierSKU = sourceDto.SupplierSKU,
                ManufacturerSKU = sourceDto.ManufacturerSKU,
                PackSize = sourceDto.PackSize,
                MinimumOrderQuantity = sourceDto.MinimumOrderQuantity,
                Notes = sourceDto.Notes,
                RoHS = sourceDto.RoHS
            };

            part.SupplySources.Add(supplySource);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue adding supply source");
        }

        [HttpPatch("{partCode}/remove-source")]
        public async Task<ActionResult> RemoveSupplySource (string partCode,
            [FromQuery]string supplierName,
            [FromQuery]string supplierSKU,
            [FromQuery]int? sourceId)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            SupplySource source;

            if (sourceId == null)
            {
                source = part.SupplySources
                    .FirstOrDefault(s => s.Supplier.Name == supplierName && s.SupplierSKU == supplierSKU);
                if (source == null) return NotFound("Couldn't find a supply source with thoughs values");
            } else
            {
                source = part.SupplySources
                    .FirstOrDefault(s => s.Id == sourceId);
                if (source == null) return NotFound("Couldn't find a supply source with that id");
            }

            _unitOfWork.SupplySourceRepository.RemoveSupplySource(source);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue removing supply source");
        }
        
        [HttpPatch("{partCode}/add-price")]
        public async Task<ActionResult> AddSourcePrice (string partCode, [FromBody] SourcePriceDto priceDto)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            var supplySource = part.SupplySources
                .FirstOrDefault(s => s.Supplier.NormalizedName == priceDto.SupplierName.ToUpper()
                                && s.SupplierSKU == priceDto.SupplierSKU);
            if (supplySource == null) return NotFound("Couldn't find a supply source with those parameters");

            if (priceDto.UnitPrice <= 0) return BadRequest("You must provide a valid unit price");
            if (priceDto.Quantity <= 0) return BadRequest("You must provide a valid quantity");

            var price = new SourcePrice
            {
                UnitPrice = priceDto.UnitPrice,
                Quantity = priceDto.Quantity
            };

            if (supplySource.Prices.ContainsWhere(p => p.UnitPrice == price.UnitPrice && p.Quantity == price.Quantity))
                return BadRequest("This price break already exists");

            supplySource.Prices.Add(price);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue adding supply source");
        }

        [HttpPatch("{partCode}/remove-price")]
        public async Task<ActionResult> RemoveSourcePrice (string partCode, [FromQuery]int? priceId)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            var price = part.SupplySources.Select(s =>
                s.Prices.Where(p => p.Id == priceId).FirstOrDefault()).FirstOrDefault();

            if (price == null) return NotFound("Couldn't find a price break by that Id for this part");

            _unitOfWork.SourcePriceRepository.RemoveSourcePrice(price);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue removing price break");
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeletePart([FromQuery]string partCode)
        {
            await _unitOfWork.PartsRepository.RemovePartByPartCode(partCode);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue deleting part");
        }


        /*
        [HttpPost("generate-parts")]
        public async Task<ActionResult> GenerateRandomData()
        {
            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName("Rapid");
            for (int i = 0; i < 20; i++)
            {
                var newPart = new Part();
                newPart.PartCode = new Random().Next(1000, 9999).ToString();
                newPart.Description = new Random().Next(1, 1000).ToString() + " Resistor";

                if (!(await _unitOfWork.PartsRepository.Exists(newPart.PartCode))) {
                    _unitOfWork.PartsRepository.AddPart(_mapper.Map<Part>(newPart));
                }
            }
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Nothing changes");
        }

        */
        [HttpGet("import-data")]
        public async Task<ActionResult<Part>> Import()
        {
            await Importer.Begin(_unitOfWork);
            return Ok();
        }
    }
}