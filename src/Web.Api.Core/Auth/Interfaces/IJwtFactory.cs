using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Auth.Interfaces
{
    public interface IJwtFactory
    {
        Task<Token> GenerateEncodedToken(string id, string userName);
    }
}
