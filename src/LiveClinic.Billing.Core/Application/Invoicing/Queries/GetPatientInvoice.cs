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
    public class GetPatientInvoice:IRequest<Result<List<InvoiceSummaryDto>>>
    {
        public string Patient { get;  }

        public GetPatientInvoice(string patient)
        {
            Patient = patient;
        }
    }

    public class GetPatientInvoiceHandler : IRequestHandler<GetPatientInvoice, Result<List<InvoiceSummaryDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public GetPatientInvoiceHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public Task<Result<List<InvoiceSummaryDto>>> Handle(GetPatientInvoice request, CancellationToken cancellationToken)
        {
            try
            {
                var invoices = _invoiceRepository.LoadAll(x => x.Patient.ToLower() == request.Patient.ToLower());

                var dtos = _mapper.Map<List<InvoiceSummaryDto>>(invoices);

                return Task.FromResult(Result.Success(dtos));
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(e, msg);
                return Task.FromResult(Result.Failure<List<InvoiceSummaryDto>>(msg));
            }
        }
    }
}
