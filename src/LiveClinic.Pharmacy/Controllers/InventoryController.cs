using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Application.Inventory.Commands;
using LiveClinic.Pharmacy.Core.Application.Inventory.Dtos;
using LiveClinic.Pharmacy.Core.Application.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _mediator.Send(new GetInventoryStats());

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
    }
}
