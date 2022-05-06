using System.Reflection.Metadata.Ecma335;
using API.Extensions;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("{productTitle}/boms")]
        public async Task<ActionResult<BOM>> GetBOMs(string productTitle)
        {
            var list = await _unitOfWork.BOMsRepository.GetAllBOMs();
            if (list == null) return NotFound("No parts lists found");
            return Ok("list");
        }

        [HttpGet("{productTitle}/boms/{title}")]
        public async Task<ActionResult<BOM>> GetBOM(string title)
        {
            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");
            return Ok(list);
        }

        [HttpPost("{productTitle}/boms")]
        public async Task<ActionResult<BOM>> AddBOM([FromBody] NewBOMDto newBOM)
        {
            
            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(newBOM.Title);
            if (list != null) return BadRequest("A part list already exists with that title");

            list = new BOM
            {
                Title = newBOM.Title,
                Description = newBOM.Description
            };

            _unitOfWork.BOMsRepository.AddNewBOM(list);
            

            if (await _unitOfWork.Complete()) return Ok(list);

            return BadRequest("Issue creating new parts list");
        }

        [HttpDelete("{productTitle}/boms/{title}")]
        public async Task<ActionResult<BOM>> RemoveBOM(string title)
        {
            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            _unitOfWork.BOMsRepository.RemoveBOM(list);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Issue deleting parts list");
        }

        [HttpPost("{productTitle}/boms/{title}/parts")]
        public async Task<ActionResult<BOM>> AddPartToList(string title, [FromBody] NewBOMEntryDto newEntry)
        {
            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");
            
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(newEntry.PartCode);
            if (part == null) return NotFound("Couldn't find a part by that partcode");

            if (list.Parts.ContainsWhere(p => p.PartId == part.Id)) return BadRequest("That part already has a quantity listed");

            var entry = new BOMEntry
            {
                PartId = part.Id,
                BOMId = list.Id,
                Quantity = newEntry.Quantity
            };

            list.Parts.Add(entry);

            if(await _unitOfWork.Complete()) return Ok(list);
            
            return BadRequest("Issue adding part to list");
        }
    }
}