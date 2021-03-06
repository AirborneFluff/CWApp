using System.Reflection.Metadata.Ecma335;
using API.DTOs.ProductsDTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public partial class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductsRepository.GetAllProducts();
            if (products == null) return NotFound("No products exists");

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewProduct([FromBody] NewProductDto newProduct)
        {
            if (await _unitOfWork.ProductsRepository.Exists(newProduct.Name)) return BadRequest("Product with this name already exists");
            var product = new Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description
            };
            _unitOfWork.ProductsRepository.AddNewProduct(product);
            if (await _unitOfWork.Complete()) return Ok(product);

            return BadRequest("Problem creating new product");
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) NotFound("No product found by that name");
            _unitOfWork.ProductsRepository.RemoveProduct(product);
            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem removing product");
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProduct(int productId)
        {
            var product = await _unitOfWork.ProductsRepository.GetProduct(productId);
            if (product == null) return NotFound("No products exists");

            return Ok(product);
        }
    }
}