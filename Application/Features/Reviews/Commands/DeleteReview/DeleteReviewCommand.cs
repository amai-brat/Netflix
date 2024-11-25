using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(long ReviewId) : ICommand<Review>;