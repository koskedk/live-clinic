using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate.Events;
using LiveClinic.SharedKernel.Common;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Core.Application.Invoicing.Commands
{
    public class ReceivePayment:IRequest<Result>
    {
        public PaymentDto PaymentDto { get; }
        public ReceivePayment(PaymentDto paymentDto)
        {
            PaymentDto = paymentDto;
        }
    }

    public class ReceivePaymentHandler : IRequestHandler<ReceivePayment, Result>
    {
        private readonly IMediator _mediator;
        private readonly IInvoiceRepository _invoiceRepository;


        public ReceivePaymentHandler(IMediator mediator, IInvoiceRepository invoiceRepository)
        {
            _mediator = mediator;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Result> Handle(ReceivePayment request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = new Payment(Money.FromAmount(request.PaymentDto.Amount), request.PaymentDto.InvoiceId);

                var invoice =  _invoiceRepository
                    .LoadAll(x=>x.Id==request.PaymentDto.InvoiceId)
                    .FirstOrDefault();

                if (null == invoice)
                    throw new Exception("Invoice not found!");

                invoice.MakePayment(payment);
                var items = invoice.Items;
                invoice.Clear();
                await _invoiceRepository.CreateOrUpdateAsync(invoice);
                await _invoiceRepository.CreateOrUpdateAsync<Payment, Guid>(new[] { payment });

                if (invoice.Status == InvoiceStatus.Paid)
                    await _mediator.Publish(new PaymentReceived(invoice.OrderId, invoice.Id, payment.Id));


                return Result.Success();
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(msg, e);
                return Result.Failure(e.Message);
            }
        }
    }
}
