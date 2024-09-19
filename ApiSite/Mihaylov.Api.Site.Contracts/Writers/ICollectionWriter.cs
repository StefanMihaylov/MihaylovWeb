using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Writers
{
    public interface ICollectionWriter
    {
        Task<AccountType> AddAccountTypeAsync(AccountType imput);
    }
}
