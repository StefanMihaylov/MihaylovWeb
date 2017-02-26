using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
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
