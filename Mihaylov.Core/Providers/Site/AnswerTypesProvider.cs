using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
{
    public class AnswerTypesProvider : IAnswerTypesProvider
    {
        private readonly IGetAllRepository<AnswerType> repository;

        public AnswerTypesProvider(IGetAllRepository<AnswerType> typeRepository)
        {
            this.repository = typeRepository;
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            IEnumerable<AnswerType> answerType = this.repository.GetAll()
                                                                .ToList();
            return answerType;
        }
    }
}
