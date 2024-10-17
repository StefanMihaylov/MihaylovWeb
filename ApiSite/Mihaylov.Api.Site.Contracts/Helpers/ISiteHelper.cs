using System;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Hubs;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Helpers
{
    public interface ISiteHelper
    {
        Task FillNewPersonAsync(Person person, string username);

        string GetUserName(string url);

        Task UpdateAccountsAsync(int? batchSize, int delay, Action<UpdateProgressBarModel> progress);
    }
}
