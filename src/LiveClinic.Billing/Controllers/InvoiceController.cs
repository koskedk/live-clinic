using System;
using System.Threading.Tasks;
using LiveClinic.Billing.Core.Application.Invoicing.Commands;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Application.Invoicing.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Billing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _mediator.Send(new GetInvoice());

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

        [HttpGet("{patient}")]
        public async Task<IActionResult> Get(string patient)
        {
            try
            {
                var results = await _mediator.Send(new GetPatientInvoice(patient));

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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderInvoiceDto invoiceDto)
        {
            if (null == invoiceDto)
                return BadRequest();

            try
            {
                var results = await _mediator.Send(new GenerateInvoice(invoiceDto));

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

        [HttpPost("pay")]
        public async Task<IActionResult> PostPay([FromBody] PaymentDto paymentDto)
        {
            if (null == paymentDto)
                return BadRequest();

            try
            {
                var results = await _mediator.Send(new ReceivePayment(paymentDto));

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
