using Grpc.Core;
using Payment;

namespace PaymentService.Services;

public class PaymentService(
    ILogger<PaymentService> logger) : Payment.PaymentService.PaymentServiceBase
{
    public override async Task ProcessPayment(
        PaymentRequest request, 
        IServerStreamWriter<PaymentResponse> responseStream, 
        ServerCallContext context)
    {
        await responseStream.WriteAsync(new PaymentResponse
        {
            TransactionId = Guid.NewGuid().ToString(),
            Status = PaymentResponse.Types.Status.Pending
        });

        await Task.Delay(5000);
        
        await responseStream.WriteAsync(new PaymentResponse
        {
            TransactionId = Guid.NewGuid().ToString(),
            Status = PaymentResponse.Types.Status.Success
        });
    }

    public override Task<CompensationResponse> CompensatePayment(
        CompensationRequest request, 
        ServerCallContext context)
    {
        logger.LogInformation("Comnsate for {TransactionId} was called", request.TransactionId);
        return base.CompensatePayment(request, context);
    }
}