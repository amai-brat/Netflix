using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.DeleteSubscription;

public record DeleteSubcriptionCommand(int Id) : ICommand<Subscription>;