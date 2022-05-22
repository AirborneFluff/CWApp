namespace API.Controllers
{
    public partial class ProductsController
    {
        [HttpGet("{productId}/boms")]
        public async Task<ActionResult<BOM>> GetBOMs(int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOMs(product);
            if (list == null) return NotFound("No parts lists found");
            return Ok(list);
        }

        [HttpGet("{productId}/boms/{title}")]
        public async Task<ActionResult<BOM>> GetBOM(string title, int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var partsList = await _unitOfWork.BOMsRepository.GetBOMFromTitle(title);
            if (partsList == null) return NotFound("Couldn't find a parts list with that title");
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

        [HttpDelete("{productId}/boms/{title}")]
        public async Task<ActionResult<BOM>> RemoveBOM(string title, int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that id");

            var list = await _unitOfWork.BOMsRepository.GetBOMFromTitle(title);
            if (list == null) return NotFound("Couldn't find a parts list with that title");

            _unitOfWork.BOMsRepository.RemoveBOM(list);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Issue deleting parts list");
        }

        [HttpPost("{productId}/boms/{title}/parts")]
        public async Task<ActionResult<BOM>> AddPartToList(int productId, string title, [FromBody] NewBOMEntryDto newEntry)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) return NotFound("Couldn't find a product by that id");

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