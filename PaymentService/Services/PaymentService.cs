using Grpc.Core;
using Payment;
using PaymentService.Data.Abstractions;
using PaymentService.Helpers;
using PaymentService.Models;
using Status = Grpc.Core.Status;

namespace PaymentService.Services;

public class PaymentService(
    ILogger<PaymentService> logger,
    IAccountProvider accountProvider,
    ITransactionRepository transactionRepository,
    ICurrencyRepository currencyRepository,
    IUnitOfWork unitOfWork) : Payment.PaymentService.PaymentServiceBase
{
    public override async Task<PaymentResponse> ProcessPayment(
        PaymentRequest request, 
        ServerCallContext context)
    {
        try
        {
            var card = request.Card.ToCardEntity();

            var currency = await currencyRepository.GetCurrencyAsync(request.CurrencyCode);
            if (currency == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Currency not found"));
            }

            var accNumberFrom = await accountProvider.GetAccountNumber(card);
            if (accNumberFrom == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Account number not found"));
            }

            var accNumberTo = await accountProvider.GetOrganizationAccountNumber();

            var transaction = Transaction.Create(
                request.UserId,
                accNumberFrom,
                accNumberTo,
                GetReasonType(request.Reason),
                currency,
                request.Amount,
                status: Dice.Flip<TransactionStatus>(0, 3)
                );

            transaction = await transactionRepository.AddTransaction(transaction);
            await unitOfWork.SaveChangesAsync(context.CancellationToken);

            return new PaymentResponse
            {
                TransactionId = transaction.Id.ToString(),
                Status = MapStatus(transaction.Status)
            };
        }
        catch (Exception ex)
        {
            logger.LogWarning("During payment process error occured: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, "Internal error"));
        }
    }

    public override async Task<CompensationResponse> CompensatePayment(
        CompensationRequest request, 
        ServerCallContext context)
    {
        logger.LogInformation("Compensate for {TransactionId} was called", request.TransactionId);
        var transaction = await transactionRepository.FindTransaction(Guid.Parse(request.TransactionId));
        if (transaction is not null)
        {
            transaction.Status = TransactionStatus.Rejected;
            await unitOfWork.SaveChangesAsync(context.CancellationToken);
        }

        return new CompensationResponse();
    }

    public override async Task<TransactionStatusResponse> GetTransactionStatus(
        TransactionStatusRequest request, 
        ServerCallContext context)
    {
        var transaction = await transactionRepository.FindTransaction(Guid.Parse(request.TransactionId));
        if (transaction == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Transaction not found"));
        }

        transaction.Status = Dice.Flip()
            ? TransactionStatus.Completed
            : TransactionStatus.Failed;
        
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
        
        return new TransactionStatusResponse
        {
            TransactionId = transaction.Id.ToString(),
            Status = MapStatus(transaction.Status)
        };
    }

    private static ReasonType GetReasonType(string reason)
    {
        return reason.Contains("subscription", StringComparison.CurrentCultureIgnoreCase)
            ? ReasonType.Subscription
            : ReasonType.Other;
    }

    private static Payment.Status MapStatus(TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.Failed => Payment.Status.Failed,
            TransactionStatus.Pending => Payment.Status.Pending,
            TransactionStatus.Completed => Payment.Status.Success,
            TransactionStatus.Rejected => Payment.Status.Failed,
            TransactionStatus.Refaunded => Payment.Status.Failed,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}