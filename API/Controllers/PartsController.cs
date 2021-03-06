using System.Collections.ObjectModel;
using System.Security.Claims;
using API.DTOs.RequisitionDTOs;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    //[Authorize]
    public class PartsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PartsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._mapper = mapper;
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

        [HttpGet("partcodes")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllPartcodes()
        {
            var parts = await _unitOfWork.PartsRepository.GetAllPartCodes();
            if (parts == null) return NotFound("No parts found...");
            return Ok(parts);
        }


        [HttpGet("{partCode}")]
        public async Task<ActionResult<Part>> GetPart(string partCode)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            return Ok(part);
        }

        [HttpDelete("{partCode}")]
        public async Task<ActionResult> DeletePart(string partCode)
        {
            await _unitOfWork.PartsRepository.RemovePartByPartCode(partCode);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue deleting part");
        }

        [HttpPost]
        public async Task<ActionResult> CreatePart([FromBody] NewPartDto part)
        {
            if (await _unitOfWork.PartsRepository.Exists(part.PartCode)) return BadRequest("A part already exists with that partcode");

            _unitOfWork.PartsRepository.AddPart(part);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem creating part");
        }

        [HttpPost("{partCode}/sources")]
        public async Task<ActionResult> AddSupplySource(string partCode, [FromBody] SupplierNameDto name)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) return NotFound("Couldn't find a part with that partcode");

            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(name.Name);
            if (supplier == null) return NotFound("Couldn't find a supplier with that name. Create one first");

            var supplySource = new SupplySource
            {
                Supplier = supplier
            };

            part.SupplySources.Add(supplySource);

            if (await _unitOfWork.Complete()) return Ok(supplySource);

            return BadRequest("Issue adding supply source");
        }

        [HttpPut("{partCode}/sources/{sourceId}")]
        public async Task<ActionResult> UpdateSupplySource(string partCode, string sourceId, [FromBody] UpdateSupplySourceDto sourceDto)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            if (sourceDto.SupplierName != null)
            {
                var newSupplier = await _unitOfWork.SuppliersRepository.GetSupplierByName(sourceDto.SupplierName);
                if (newSupplier == null) return NotFound("Couldn't find a supplier with that name");

                supplySource.Supplier = newSupplier;
            }

            _mapper.Map(sourceDto, supplySource);

            _unitOfWork.SupplySourceRepository.Update(supplySource);

            if (await _unitOfWork.Complete()) return Ok(supplySource);

            return BadRequest("Issue updating supply source, did you make any changes?");
        }

        [HttpDelete("{partCode}/sources/{sourceId}")]
        public async Task<ActionResult> RemoveSupplySource(string partCode, string sourceId)
        {

            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            _unitOfWork.SupplySourceRepository.RemoveSupplySource(supplySource);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Issue removing supply source");
        }

        [HttpPost("{partCode}/sources/{sourceId}/prices")]
        public async Task<ActionResult<SourcePrice>> AddSourcePrice(string partCode, string sourceId, [FromBody] Price priceDto)
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

        [HttpPut("{partCode}/sources/{sourceId}/prices")]
        public async Task<ActionResult> UpdateSourcePrices(string partCode, string sourceId, [FromBody] Price[] newPrices)
        {
            var result = await GetPartAndSource(partCode, sourceId);
            if (result.Value == null) return result.Result;

            var part = result.Value.Item1;
            var supplySource = result.Value.Item2;

            foreach (var elem in supplySource.Prices)
                _unitOfWork.SourcePriceRepository.RemoveSourcePrice(elem);

            var newArr = new Collection<SourcePrice>();

            bool validCheck = true;

            foreach (var elem in newPrices)
            {
                if (elem.UnitPrice < 0) { validCheck = false; break; }
                if (elem.Quantity <= 0) { validCheck = false; break; }

                var newPrice = new SourcePrice
                {
                    UnitPrice = elem.UnitPrice,
                    Quantity = elem.Quantity
                };

                if (newArr.ContainsWhere(p => p.UnitPrice == newPrice.UnitPrice && p.Quantity == newPrice.Quantity)) break;

                newArr.Add(newPrice);
            }

            supplySource.Prices = newArr;

            if (await _unitOfWork.Complete())
            {
                if (validCheck) return Ok();
                return Ok("One or more price breaks where not added, make sure a valid Quantity and UnitPrice is provided for each entry");
            }

            return BadRequest("Issue editting prices");
        }

        [HttpDelete("{partCode}/sources/{sourceId}/prices/{priceId}")]
        public async Task<ActionResult> RemoveSourcePrice(string partCode, string sourceId, string priceId)
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

        [HttpGet("{partCode}/requisitions")]
        public async Task<ActionResult<IEnumerable<RequisitionDetailsDto>>> GetRequisitionsForPart(string partCode, [FromQuery] bool latest)
        {
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(partCode);
            if (part == null) NotFound("No part found by that partcode");

            if (latest)
            {
                var requistion = await _unitOfWork.RequisitionsRepository.GetOpenRequisitionForPart(part.Id);
                if (requistion == null) NotFound("No open requisitions for this part");
                return Ok(_mapper.Map<RequisitionDetailsDto>(requistion));
            }

            var requisitions = await _unitOfWork.RequisitionsRepository.GetRequisitionsForPart(part.Id);
            if (requisitions == null) return NotFound("No requisitions exist for this part");
            return Ok(_mapper.Map<RequisitionDetailsDto>(requisitions));
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