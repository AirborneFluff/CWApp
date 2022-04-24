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
        public async Task<ActionResult> AddSupplySource (string partCode, [FromQuery] string supplierName)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(supplierName);
            if (supplier == null) return NotFound("Couldn't find a supplier with that name. Create one first");

            var supplySource = new SupplySource
            {
                Supplier = supplier
            };

            part.SupplySources.Add(supplySource);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue adding supply source");
        }

        [HttpPatch("{partCode}/{sourceId}")]
        public async Task<ActionResult> UpdateSupplySource (string partCode, string sourceId, [FromBody] UpdateSupplySourceDto sourceDto)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            var newSupplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(sourceDto.SupplierName);
            if (newSupplier == null) return NotFound("Couldn't find a supplier with that name");

            supplySource.Supplier = newSupplier;
            supplySource.SupplierSKU = sourceDto.SupplierSKU;
            supplySource.ManufacturerSKU = sourceDto.ManufacturerSKU;
            supplySource.PackSize = sourceDto.PackSize;
            supplySource.MinimumOrderQuantity = sourceDto.MinimumOrderQuantity;
            supplySource.Notes = sourceDto.Notes;
            supplySource.RoHS = sourceDto.RoHS;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue updating supply source, did you make any changes?");
        }

        [HttpPatch("{partCode}/{sourceId}/remove-source")]
        public async Task<ActionResult> RemoveSupplySource (string partCode, string sourceId)
        {
            
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            _unitOfWork.SupplySourceRepository.RemoveSupplySource(supplySource);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue removing supply source");
        }
        
        [HttpPost("{partCode}/{sourceId}/add-price")]
        public async Task<ActionResult<SourcePrice>> AddSourcePrice (string partCode, string sourceId, [FromBody] SourcePrice priceDto)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            if (priceDto.UnitPrice < 0) return BadRequest("You must provide a valid unit price");
            if (priceDto.Quantity <= 0) return BadRequest("You must provide a valid quantity");

            var price = new SourcePrice
            {
                UnitPrice = priceDto.UnitPrice,
                Quantity = priceDto.Quantity
            };

            if (supplySource.Prices.ContainsWhere(p => p.UnitPrice == price.UnitPrice && p.Quantity == price.Quantity))
                return BadRequest("This price break already exists");

            supplySource.Prices.Add(price);

            if (await _unitOfWork.Complete())
                return Ok(supplySource.Prices.FirstOrDefault(x => x.PriceString == price.PriceString));

            return BadRequest("Issue adding supply source");
        }

        [HttpPatch("{partCode}/{sourceId}/update-prices")]
        public async Task<ActionResult> UpdateSourcePrices (string partCode, string sourceId, [FromBody] Price[] newPrices)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            foreach(var elem in supplySource.Prices)
                _unitOfWork.SourcePriceRepository.RemoveSourcePrice(elem);

            var newArr = new Collection<SourcePrice>();

            foreach (var elem in newPrices)
            {
                if (elem.UnitPrice < 0) break;
                if (elem.Quantity <= 0) break;

                var newPrice = new SourcePrice
                {
                    UnitPrice = elem.UnitPrice,
                    Quantity = elem.Quantity
                };

                if (newArr.ContainsWhere(p => p.UnitPrice == newPrice.UnitPrice && p.Quantity == newPrice.Quantity)) break;

                newArr.Add(newPrice);
            }

            supplySource.Prices = newArr;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue editting prices");
        }

        [HttpPatch("{partCode}/{sourceId}/remove-price")]
        public async Task<ActionResult> RemoveSourcePrice (string partCode, string sourceId, [FromQuery]string priceId)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            int priceIdVal = 0;
            if (!int.TryParse(priceId, out priceIdVal)) return BadRequest("Price Id must be a number");

            var price = await _unitOfWork.SourcePriceRepository.GetSourcePriceById(priceIdVal);
            if (price == null) return NotFound("Couldn't find a price break by that Id");

            if (!supplySource.Prices.Contains(price)) return BadRequest("This price does not belong to that supply source");

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

        private async Task<ActionResult<Tuple<Part, SupplySource>>> GetPartAndSource(string partCode, string sourceId)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            int sourceIdVal = 0;
            if (!int.TryParse(sourceId, out sourceIdVal)) return BadRequest("Source Id must be a number");

            var supplySource = await _unitOfWork.SupplySourceRepository.GetSupplySourceById(sourceIdVal);
            if (supplySource == null) return NotFound("Couldn't find a supply source with that Id");

            if (!part.SupplySources.Contains(supplySource)) return BadRequest("This supply source does not belong to this partcode");

            return new Tuple<Part, SupplySource>(part, supplySource);
        }
    }
}