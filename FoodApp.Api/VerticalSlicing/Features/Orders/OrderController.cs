﻿namespace FoodApp.Api.VerticalSlicing.Features.Orders
{
    public class OrderController : BaseController
    {
        public OrderController(ControllerParameters controllerParameters) : base(controllerParameters) { }

        [HttpPost("CreateOrder")]
        public async Task<Result<CreateOrderResponse>> MakeOrder(CreateOrderRequest request)
        {
            var command = request.Map<CreateOrderCommand>();
            var result = await _mediator.Send(command);
            return result;
        }
        [HttpPost("UpdateOrderStatus")]
        public async Task<Result<bool>> UpdateOrderStatus([FromForm]UpdateOrderStatusRequest request)
        {
            var command = request.Map<UpdateOrderStatusCommand>();
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet("accept/{orderId}")]
        public async Task<Result> AcceptOrder(int orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand(orderId, OrderStatus.Accepted));

            return result;
        }

        [HttpGet("reject/{orderId}")]
        public async Task<Result> RejectOrder(int orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand(orderId, OrderStatus.Rejected));

            return result;
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<Result> CancelOrder(int orderId)
        {
            var result = await _mediator.Send(new CancelOrderCommand(orderId));
            return result;

        }
        [HttpPost("AssignOrdersToDeliveryMan")]
        public async Task<Result> AssignOrderToDeliveryMan(AssignOrdersToDeliveryManRequest request)
        {
            var command = request.Map<AssignOrdersToDeliveryManCommand>();
            var result = await _mediator.Send(command);
            return result;
        }
        [HttpPut("UpdateOrderTripStatus")]
        public async Task<Result<bool>> UpdateOrderTripStatus([FromForm]UpdateOrderStatusTripRequest request)
        {
            var command = request.Map<UpdateOrderStatusTripCommand>();
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
