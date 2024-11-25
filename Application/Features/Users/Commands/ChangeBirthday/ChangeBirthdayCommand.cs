using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Users.Commands.ChangeBirthday;

public record ChangeBirthdayCommand(long UserId, DateOnly NewBirthday) : ICommand<User>;