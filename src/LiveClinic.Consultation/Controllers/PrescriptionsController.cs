using System;
using System.Threading.Tasks;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Consultation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] PrescriptionDto orderDto)
        {
            if (null == orderDto)
                return BadRequest();

            try
            {
                var results = await _mediator.Send(new PrescribeDrugs(orderDto));

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
