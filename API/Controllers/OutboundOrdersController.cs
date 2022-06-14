using API.DTOs.OutboundOrderDTOs;

namespace API.Controllers
{
    public class OutboundOrdersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OutboundOrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        // Get an order
        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrderById(int orderId)
        {
            var order = await _unitOfWork.OutboundOrdersRepository.GetOutboundOrderById(orderId);
            if (order == null) NotFound("That order doesn't exist");

            return Ok(order);
        }

        // Create an order
        [HttpPost]
        public async Task<ActionResult> CreateOrder(NewOutboundOrderDTO orderDto)
        {
            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierById(orderDto.SupplierId);
            if (supplier == null) return NotFound("No supplier found with that Id");

            var newOrder = _mapper.Map<OutboundOrder>(orderDto);
            _unitOfWork.OutboundOrdersRepository.AddOrder(newOrder);

            if(await _unitOfWork.Complete()) return Ok(newOrder);

            return BadRequest("Unable to create new order");
        }

        // Delete an order
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var order = await _unitOfWork.OutboundOrdersRepository.GetOutboundOrderById(orderId);
            if (order == null) BadRequest("That order doesn't exist");

            _unitOfWork.OutboundOrdersRepository.RemoveOrder(order);

            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Unable to delete order");
        }

        // Add an item to order
        [HttpPost("{orderId}/items")]
        public async Task<ActionResult> AddItem(int orderId, [FromBody] NewOutboundOrderItemDTO itemDto)
        {
            var order = await _unitOfWork.OutboundOrdersRepository.GetOutboundOrderById(orderId);
            if (order == null) BadRequest("That order doesn't exist");

            var part = await _unitOfWork.PartsRepository.GetPartByPartCode(itemDto.Partcode);
            if (part == null) BadRequest("That part doesn't exist");

            var newItem = new OutboundOrderItem
            {
                OutboundOrderId = order.Id,
                Part = part,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            };

            order.Items.Add(newItem);

            if(await _unitOfWork.Complete()) return Ok(order);

            return BadRequest("Unable to add new item for order");
        }

        // Delete item from order
        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<ActionResult> DeleteItem(int orderId, int itemId)
        {
            var order = await _unitOfWork.OutboundOrdersRepository.GetOutboundOrderById(orderId);
            if (order == null) BadRequest("That order doesn't exist");

            var item = order.Items.FirstOrDefault(i => i.Id == itemId);

            order.Items.Remove(item);

            if(await _unitOfWork.Complete()) return Ok(order);

            return BadRequest("Unable to add new item for order");
        }

        // Modify item in order
        [HttpPut("{orderId}/items/{itemId}")]
        public async Task<ActionResult> ModifyItem(int orderId, int itemId, [FromBody] Price price)
        {
            var order = await _unitOfWork.OutboundOrdersRepository.GetOutboundOrderById(orderId);
            if (order == null) BadRequest("That order doesn't exist");

            var item = order.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) BadRequest("That item doesn't exist");

            item.Quantity = price.Quantity;
            item.UnitPrice = price.UnitPrice;

            if(await _unitOfWork.Complete()) return Ok(order);

            return BadRequest("Unable to add new item for order");
        }
    }
}