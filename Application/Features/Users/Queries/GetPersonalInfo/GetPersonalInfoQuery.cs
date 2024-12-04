using Application.Cqrs.Queries;

namespace Application.Features.Users.Queries.GetPersonalInfo;

public record GetPersonalInfoQuery(long UserId) : IQuery<PersonalInfoDto>;