using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Common.Validations;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Managers
{
    public class AnswerTypesManager : IAnswerTypesManager
    {
        protected readonly IAnswerTypesProvider provider;

        protected readonly ConcurrentDictionary<int, AnswerType> answerTypesById;
        protected readonly ConcurrentDictionary<string, AnswerType> answerTypesByName;

        public AnswerTypesManager(IAnswerTypesProvider answerTypeProvider)
        {
            ParameterValidation.IsNotNull(answerTypeProvider, nameof(answerTypeProvider));

            this.provider = answerTypeProvider;

            this.answerTypesById = new ConcurrentDictionary<int, AnswerType>();
            this.answerTypesByName = new ConcurrentDictionary<string, AnswerType>(StringComparer.OrdinalIgnoreCase);

            Initialize();
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            IEnumerable<AnswerType> answerTypes = this.answerTypesById.Values;
            return answerTypes;
        }

        public AnswerType GetById(int id)
        {
            AnswerType type;
            if (this.answerTypesById.TryGetValue(id, out type))
            {
                return type;
            }
            else
            {
                throw new ApplicationException($"AnswerType with id: {id} was not found");
            }
        }

        public AnswerType GetByName(string name)
        {
            name = name.Trim();
            AnswerType type;
            if (this.answerTypesByName.TryGetValue(name, out type))
            {
                return type;
            }
            else
            {
                throw new ApplicationException($"AnswerType with name: {name} was not found");
            }
        }

        private void Initialize()
        {
            IEnumerable<AnswerType> answerTypes = this.provider.GetAllAnswerTypes();
            foreach (var answerType in answerTypes)
            {
                this.answerTypesById.TryAdd(answerType.Id, answerType);
                this.answerTypesByName.TryAdd(answerType.Name, answerType);
            }
        }
    }
}
