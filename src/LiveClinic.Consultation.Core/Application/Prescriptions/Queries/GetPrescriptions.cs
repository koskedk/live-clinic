using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using MediatR;
using Serilog;

namespace LiveClinic.Consultation.Core.Application.Prescriptions.Queries
{
    public class GetPrescriptions : IRequest<Result<List<Prescription>>>
    {
        public string Patient { get; }
        public Guid? OrderId  { get; }

        public GetPrescriptions(Guid? orderId = null, string patient = "")
        {
            Patient = patient;
            OrderId = orderId;
        }
    }

    public class GetOrdersHandler : IRequestHandler<GetPrescriptions,Result<List<Prescription>>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetOrdersHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public Task<Result<List<Prescription>>> Handle(GetPrescriptions request, CancellationToken cancellationToken)
        {
            try
            {
                var drugOrders=new List<Prescription>();

                if (request.OrderId.HasValue)
                {
                    drugOrders = _prescriptionRepository.LoadAll(x => x.Id == request.OrderId).ToList();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.Patient))
                        drugOrders = _prescriptionRepository.LoadAll().ToList();
                    else
                        drugOrders = _prescriptionRepository.LoadAll(x => x.Patient == request.Patient).ToList();
                }

                return Task.FromResult(Result.Success(drugOrders));
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(e, msg);
                return Task.FromResult(Result.Failure<List<Prescription>>(msg));
            }
        }
    }
}
