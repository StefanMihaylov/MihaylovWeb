using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IParserSettingRepository
    {
        Task<IEnumerable<ParserSetting>> GetAllAsync();

        Task<ParserSetting> AddOrUpdateAsync(ParserSetting model);        
    }
}