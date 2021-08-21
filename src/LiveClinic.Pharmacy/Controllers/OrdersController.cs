using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Application.Inventory.Commands;
using LiveClinic.Pharmacy.Core.Application.Inventory.Dtos;
using LiveClinic.Pharmacy.Core.Application.Inventory.Queries;
using LiveClinic.Pharmacy.Core.Application.Orders.Commands;
using LiveClinic.Pharmacy.Core.Application.Orders.Queries;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Active")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var results = await _mediator.Send(new GetOrders(PrescriptionStatus.Active));

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error occured";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpGet("Active/Order/{orderId}")]
        public async Task<IActionResult> GetOrderItems(Guid orderId)
        {
            try
            {
                var results = await _mediator.Send(new GetOrders(PrescriptionStatus.Active, orderId));

                if (results.IsSuccess)
                    return Ok(results.Value.FirstOrDefault());

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error occured";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                var results = await _mediator.Send(new GetOrders(PrescriptionStatus.Closed));

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error occured";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpGet("History/Order/{orderId}")]
        public async Task<IActionResult> GetHisOrderItems(Guid orderId)
        {
            try
            {
                var results = await _mediator.Send(new GetOrders(PrescriptionStatus.Closed, orderId));

                if (results.IsSuccess)
                    return Ok(results.Value.FirstOrDefault());

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error occured";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }


        [HttpPost("FullDispense")]
        public async Task<IActionResult> Post(Guid orderId)
        {
            try
            {
                var results = await _mediator.Send(new DispenseDrugs(orderId));

                if (results.IsSuccess)
                    return Ok();

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error occured";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
