using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Core.Domain.Prescriptions.Events;
using MediatR;
using Serilog;

namespace LiveClinic.Consultation.Core.Application.Prescriptions.Commands
{
    public class PrescribeDrugs:IRequest<Result>
    {
        public PrescriptionDto OrderDto { get; }

        public PrescribeDrugs(PrescriptionDto orderDto)
        {
            OrderDto = orderDto;
        }
    }

    public class PrescribeDrugsHandler : IRequestHandler<PrescribeDrugs, Result>
    {
        private readonly IMediator _mediator;
        private readonly IPrescriptionRepository _prescriptionRepository;

        public PrescribeDrugsHandler(IMediator mediator, IPrescriptionRepository prescriptionRepository)
        {
            _mediator = mediator;
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<Result> Handle(PrescribeDrugs request, CancellationToken cancellationToken)
        {
            try
            {
                var order=Prescription.Generate(request.OrderDto);

               await _prescriptionRepository.CreateOrUpdateAsync(order);

               await _mediator.Publish(new PrescriptionGenerated(order.Id));

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
