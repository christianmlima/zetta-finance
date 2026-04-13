using Zetta.Domain.Aggregates.Users;

namespace Zetta.Application.Common.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);
}
