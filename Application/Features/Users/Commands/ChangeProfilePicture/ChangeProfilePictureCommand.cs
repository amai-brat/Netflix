using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Users.Commands.ChangeProfilePicture;

public record ChangeProfilePictureCommand(long UserId, Stream PictureStream, string ContentType) : ICommand<User>;