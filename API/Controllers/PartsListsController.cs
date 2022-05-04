using System.Reflection.Metadata.Ecma335;
using API.DTOs.PartsListDTOs;
using API.Extensions;

namespace API.Controllers
{
    public class PartsListsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PartsListsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<PartsList>> GetPartsLists()
        {
            var list = await _unitOfWork.PartsListsRepository.GetAllPartsLists();
            if (list == null) return NotFound("No parts lists found");
            return Ok(list);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<PartsList>> GetPartsList(string title)
        {
            var list = await _unitOfWork.PartsListsRepository.GetPartsListFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<PartsList>> AddPartsList([FromBody] NewPartsListDto newPartsList)
        {
            var list = await _unitOfWork.PartsListsRepository.GetPartsListFromTitle(newPartsList.Title);
            if (list != null) return BadRequest("A part list already exists with that title");

            list = new PartsList
            {
                Title = newPartsList.Title,
                Description = newPartsList.Description
            };

            _unitOfWork.PartsListsRepository.AddNewPartsList(list);

            if (await _unitOfWork.Complete()) return Ok(list);

            return BadRequest("Issue creating new parts list");
        }

        [HttpDelete("{title}")]
        public async Task<ActionResult<PartsList>> RemovePartsList(string title)
        {
            var list = await _unitOfWork.PartsListsRepository.GetPartsListFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            _unitOfWork.PartsListsRepository.RemovePartsList(list);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Issue deleting parts list");
        }

        [HttpPost("{title}/parts")]
        public async Task<ActionResult<PartsList>> AddPartToList(string title, [FromBody] NewPartsListEntryDto newEntry)
        {
            var list = await _unitOfWork.PartsListsRepository.GetPartsListFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");
            
            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(newEntry.PartCode);
            if (part == null) return NotFound("Couldn't find a part by that partcode");

            if (list.Parts.ContainsWhere(p => p.PartId == part.Id)) return BadRequest("That part already has a quantity listed");

            var entry = new PartsListEntry
            {
                PartId = part.Id,
                PartsListId = list.Id,
                Quantity = newEntry.Quantity
            };

            list.Parts.Add(entry);

            if(await _unitOfWork.Complete()) return Ok(list);
            
            return BadRequest("Issue adding part to list");
        }
    }
}