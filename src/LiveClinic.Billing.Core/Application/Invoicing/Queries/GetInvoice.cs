using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Core.Application.Invoicing.Queries
{
    public class GetInvoice:IRequest<Result<List<InvoiceSummaryDto>>>
    {
    }

    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Result<List<InvoiceSummaryDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public GetInvoiceHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public Task<Result<List<InvoiceSummaryDto>>> Handle(GetInvoice request, CancellationToken cancellationToken)
        {
            try
            {
                var invoices = _invoiceRepository.LoadAll();

                var dtos = _mapper.Map<List<InvoiceSummaryDto>>(invoices);

                return Task.FromResult(Result.Success(dtos));
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(msg, e);
                return Task.FromResult(Result.Failure<List<InvoiceSummaryDto>>(msg));
            }
        }
    }
}
