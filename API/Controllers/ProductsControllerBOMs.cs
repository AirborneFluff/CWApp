using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public partial class ProductsController
    {
        [Authorize]
        [HttpGet("{productId}/boms")]
        public async Task<ActionResult<IEnumerable<BOM>>> GetBOMs(int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOMs(product);
            if (list == null) return NotFound("No parts lists found");
            return Ok(list);
        }

        [HttpGet("{productId}/boms/{bomId}")]
        public async Task<ActionResult<BOM>> GetBOM(int bomId, int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var partsList = await _unitOfWork.BOMsRepository.GetBOM(bomId);
            if (partsList == null) return NotFound("Couldn't find a parts list with that id");
            return Ok(partsList);
        }

        [HttpPost("{productId}/boms")]
        public async Task<ActionResult<BOM>> AddBOM([FromBody] NewBOMDto newBOM, int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(newBOM.Title);
            if (list != null) return BadRequest("A part list already exists with that title");

            list = new BOM
            {
                ProductId = product.Id,
                Title = newBOM.Title,
                Description = newBOM.Description
            };

            _unitOfWork.BOMsRepository.AddNewBOM(list);

            if (await _unitOfWork.Complete()) return Ok(list);

            return BadRequest("Issue creating new parts list");
        }

        [HttpDelete("{productId}/boms/{bomId}")]
        public async Task<ActionResult<BOM>> RemoveBOM(int bomId, int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOM(bomId);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            _unitOfWork.BOMsRepository.RemoveBOM(list);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Issue deleting parts list");
        }

        [HttpPost("{productId}/boms/{bomId}/parts")]
        public async Task<ActionResult<BOM>> AddPartToList(int productId, int bomId, [FromBody] NewBOMEntryDto newEntry)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) return NotFound("Couldn't find a product by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOM(bomId);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(newEntry.PartCode);
            if (part == null) return NotFound("Couldn't find a part by that partcode");

            if (list.Parts.ContainsWhere(p => p.PartId == part.Id)) return BadRequest("That part already has a quantity listed");

            var entry = new BOMEntry
            {
                PartId = part.Id,
                BOMId = list.Id,
                Quantity = newEntry.Quantity,
                ComponentLocation = newEntry.ComponentLocation
            };

            list.Parts.Add(entry);

            if (await _unitOfWork.Complete()) return Ok(list);

            return BadRequest("Issue adding part to list");
        }

        [HttpPut("{productId}/boms/{bomId}/parts/{partId}")]
        public async Task<ActionResult<BOM>> UpdateEntry(int productId, int bomId, int partId, [FromBody] UpdateBOMEntryDto entry)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) return NotFound("Couldn't find a product by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOM(bomId);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            var entryToUpdate = list.Parts.FirstOrDefault(x => x.PartId == partId);
            if (entryToUpdate == null) return NotFound("The part with that part Id isn't apart of this parts list");

            if (entry.Quantity <= 0) return BadRequest("You must provide a quantity that isn't 0");

            entryToUpdate.Quantity = entry.Quantity;
            entryToUpdate.ComponentLocation = entry.ComponentLocation;

            if (await _unitOfWork.Complete()) return Ok(list);

            return BadRequest("Issue edditing part");
        }

        [HttpDelete("{productId}/boms/{bomId}/parts/{partId}")]
        public async Task<ActionResult<BOM>> UpdateEntry(int productId, int bomId, int partId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) return NotFound("Couldn't find a product by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOM(bomId);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            var entryToDelete = list.Parts.FirstOrDefault(x => x.PartId == partId);
            if (entryToDelete == null) return NotFound("The part with that part Id isn't apart of this parts list");

            list.Parts.Remove(entryToDelete);

            if(await _unitOfWork.Complete()) return Ok(list);
            
            return BadRequest("Issue deleting part");
        }
    }
}