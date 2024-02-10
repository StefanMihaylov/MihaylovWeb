using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Data
{
    public class ExternalRepository : IExternalRepository
    {
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly ILogger _logger;

        public ExternalRepository(IAuthenticationSchemeProvider schemes, ILoggerFactory factory)
        {
            _schemes = schemes;
            _logger = factory.CreateLogger(this.GetType().Name);
        }

        public async Task<IEnumerable<SchemeModel>> GetExternalAuthenticationSchemesAsync()
        {
            var allSchemes = await _schemes.GetAllSchemesAsync().ConfigureAwait(false);
            var schemas = allSchemes.Where(s => !string.IsNullOrEmpty(s.DisplayName))
                                    .Select(s => new SchemeModel()
                                    {
                                        Name = s.Name,
                                        DisplayName = s.DisplayName,
                                        TypeName = s.HandlerType.Name,
                                    });
            return schemas;
        }
    }
}
