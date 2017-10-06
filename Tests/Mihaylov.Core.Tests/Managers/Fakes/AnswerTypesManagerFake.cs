using System.Collections.Generic;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Core.Managers.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Tests.Managers.Fakes
{
    internal class AnswerTypesManagerFake : AnswerTypesManager
    {
        public AnswerTypesManagerFake(IAnswerTypesProvider answerTypeProvider)
            : base(answerTypeProvider)
        {
        }

        public IAnswerTypesProvider ExposedProvider
        {
            get
            {
                return this.provider;
            }
        }

        public IDictionary<int, AnswerType> ExposedDictionaryById
        {
            get
            {
                return this.answerTypesById;
            }
        }

        public IDictionary<string, AnswerType> ExposedDictionaryByName
        {
            get
            {
                return this.answerTypesByName;
            }
        }
    }
}
