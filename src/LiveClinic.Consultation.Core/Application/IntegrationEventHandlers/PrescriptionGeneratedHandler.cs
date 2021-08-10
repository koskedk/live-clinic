using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Core.Domain.Prescriptions.Events;
using LiveClinic.Contracts;
using MassTransit;
using MediatR;

namespace LiveClinic.Consultation.Core.Application.IntegrationEventHandlers
{
    public class PrescriptionGeneratedHandler : INotificationHandler<PrescriptionGenerated>
    {
        private readonly IBus _bus;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMapper _mapper;

        public PrescriptionGeneratedHandler(IBus bus, IPrescriptionRepository prescriptionRepository, IMapper mapper)
        {
            _bus = bus;
            _prescriptionRepository = prescriptionRepository;
            _mapper = mapper;
        }

        public async Task Handle(PrescriptionGenerated notification, CancellationToken cancellationToken)
        {
            var order = _prescriptionRepository.LoadAll(x => x.Id == notification.Id).FirstOrDefault();

            if (null == order)
                throw new Exception("Missing Order !");

            var orderDto = _mapper.Map<OrderGenerated>(order);

            await _bus.Publish(orderDto,cancellationToken);
        }
    }
}
