using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Core.Application.Invoicing.Commands
{
    public class GenerateInvoice:IRequest<Result>
    {
        public OrderInvoiceDto InvoiceDto { get; }

        public GenerateInvoice(OrderInvoiceDto invoiceDto)
        {
            InvoiceDto = invoiceDto;
        }
    }

    public class GenerateInvoiceHandler : IRequestHandler<GenerateInvoice, Result>
    {
        private readonly IMediator _mediator;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPriceCatalogRepository _priceCatalogRepository;

        public GenerateInvoiceHandler(IMediator mediator, IInvoiceRepository invoiceRepository, IPriceCatalogRepository priceCatalogRepository)
        {
            _mediator = mediator;
            _invoiceRepository = invoiceRepository;
            _priceCatalogRepository = priceCatalogRepository;
        }

        public async Task<Result> Handle(GenerateInvoice request, CancellationToken cancellationToken)
        {
            try
            {
                var drugCodes = request.InvoiceDto.DrugCodes;

                var prices = _priceCatalogRepository
                    .GetAll(x => drugCodes.Contains(x.DrugCode))
                    .ToList();

                var invoice = Invoice.Generate(request.InvoiceDto,prices);

               await _invoiceRepository.CreateOrUpdateAsync(invoice);

               return Result.Success();
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(msg, e);
                return Result.Failure(msg);
            }
        }
    }
}
