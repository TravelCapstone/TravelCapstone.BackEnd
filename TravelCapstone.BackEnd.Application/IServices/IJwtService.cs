using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IJwtService
{
    string GenerateRefreshToken();

    Task<string> GenerateAccessToken(LoginRequestDto loginRequest);

    Task<TokenDto> GetNewToken(string refreshToken, string accountId);
}