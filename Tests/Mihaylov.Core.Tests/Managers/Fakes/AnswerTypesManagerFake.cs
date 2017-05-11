using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Data.Models.Repositories;

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
