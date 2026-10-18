using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries
{
    public record GetBrandsQuery : IRequest<IEnumerable<Brand>>;

    public class GetBrandsQueryHandler(IBrandRepository repository, ILogger<GetBrandsQueryHandler> logger) : IRequestHandler<GetBrandsQuery, IEnumerable<Brand>>
    {
        public async Task<IEnumerable<Brand>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetBrands failed. Message: {Message}", ex.Message);
                throw;
            }
        }
    }
}
