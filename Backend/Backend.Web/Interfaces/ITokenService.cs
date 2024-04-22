using Backend.Web.Models;

namespace Backend.Web.Interfaces;

public interface ITokenService
{
    public Task<string> CreateToken(User user);
}
