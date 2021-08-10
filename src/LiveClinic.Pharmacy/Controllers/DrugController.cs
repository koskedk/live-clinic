using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Application.Commands;
using LiveClinic.Pharmacy.Core.Application.Dtos;
using LiveClinic.Pharmacy.Core.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Pharmacy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrugController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DrugController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _mediator.Send(new GetInventory());

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

        [HttpPost("NewStock")]
        public async Task<IActionResult> Post([FromBody] List<NewStockDto> newStockDtos)
        {
            if (!newStockDtos.Any())
                return BadRequest();

            try
            {
                var results = await _mediator.Send(new ReceiveStock(newStockDtos));

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
