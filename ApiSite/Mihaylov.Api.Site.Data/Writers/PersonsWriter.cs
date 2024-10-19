using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Common.Generic.Extensions;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository _repository;
        private readonly IQuizRepository _quizRepository;

        public PersonsWriter(IPersonsRepository personsRepository, IQuizRepository quizRepository)
        {
            _repository = personsRepository;
            _quizRepository = quizRepository;
        }

        public Task<Person> AddOrUpdatePersonAsync(Person input, int? age)
        {
            var type = input.DateOfBirthType;
            var isCalculating = type == DateOfBirthType.YearCalculated;

            input.DateOfBirthType = input.DateOfBirth.IsBirthDateTypeValid(age, isCalculating) ? type : null;
            input.DateOfBirth = input.DateOfBirth.GetBirthDate(age, type.HasValue, isCalculating);

            return _repository.AddOrUpdatePersonAsync(input);
        }

        public Task<Account> AddOrUpdateAccountAsync(Account input, int? age)
        {
            if (!input.CreateDate.HasValue)
            {
                input.CreateDate = age.GetCreateDate();
            }

            return _repository.AddOrUpdateAccountAsync(input);
        }

        public async Task<Person> AddNewPersonAsync(Person input, int? age)
        {
            var newPerson = await AddOrUpdatePersonAsync(input, age).ConfigureAwait(false);

            var newAccounts = new List<Account>();
            newPerson.Accounts = newAccounts;

            foreach (var account in input.Accounts)
            {
                account.PersonId = newPerson.Id;
                var newAccount = await AddOrUpdateAccountAsync(account, null).ConfigureAwait(false);
                newAccounts.Add(newAccount);
            }

            return newPerson;
        }

        public async Task<Person> MergePersonsAsync(PersonMerge input)
        {
            Person from = await _repository.GetPersonAsync(input.From).ConfigureAwait(false);
            Person to = await _repository.GetPersonAsync(input.To).ConfigureAwait(false);

            if (from == null || to == null)
            {
                return null;
            }

            MergePerson(input, from, to);
            Person toUpdated = await _repository.AddOrUpdatePersonAsync(to).ConfigureAwait(false);

            if (input.Accounts == true)
            {
                foreach (var account in from.Accounts)
                {
                    account.PersonId = to.Id;
                    await _repository.AddOrUpdateAccountAsync(account).ConfigureAwait(false);
                }
            }

            if (input.Answers == true)
            {
                var answers = await _quizRepository.GetQuizAnswersAsync(from.Id).ConfigureAwait(false);
                foreach (var answer in answers)
                {
                    answer.PersonId = to.Id;
                    await _quizRepository.AddQuizAnswerAsync(answer).ConfigureAwait(false);
                }
            }

            await _repository.DeletePersonAsync(from.Id);

            return toUpdated;
        }


        private void MergePerson(PersonMerge input, Person from, Person to)
        {
            if (input.FirstName == true)
            {
                to.Details ??= new PersonDetail();
                to.Details.FirstName = from.Details?.FirstName;
            }

            if (input.MiddleName == true)
            {
                to.Details ??= new PersonDetail();
                to.Details.MiddleName = from.Details?.MiddleName;
            }

            if (input.LastName == true)
            {
                to.Details ??= new PersonDetail();
                to.Details.LastName = from.Details?.LastName;
            }

            if (input.OtherNames == true)
            {
                to.Details ??= new PersonDetail();
                to.Details.OtherNames = from.Details?.OtherNames;
            }

            if (input.DateOfBirth == true)
            {
                to.DateOfBirth = from.DateOfBirth;
            }

            if (input.DateOfBirthType == true)
            {
                to.DateOfBirthType = from.DateOfBirthType;
            }

            if (input.CountryId == true)
            {
                to.CountryId = from.CountryId;
            }

            if (input.CountryStateId == true)
            {
                to.Location ??= new PersonLocation();
                to.Location.CountryStateId = from.Location?.CountryStateId;
            }

            if (input.Region == true)
            {
                to.Location ??= new PersonLocation();
                to.Location.Region = from.Location?.Region;
            }

            if (input.City == true)
            {
                to.Location ??= new PersonLocation();
                to.Location.City = from.Location?.City;
            }

            if (input.Details == true)
            {
                to.Location ??= new PersonLocation();
                to.Location.Details = from.Location?.Details;
            }

            if (input.EthnicityId == true)
            {
                to.EthnicityId = from.EthnicityId;
            }

            if (input.OrientationId == true)
            {
                to.OrientationId = from.OrientationId;
            }

            if (input.Comments == true)
            {
                to.Comments = $"{to.Comments}|{from.Comments}";
            }
        }
    }
}
