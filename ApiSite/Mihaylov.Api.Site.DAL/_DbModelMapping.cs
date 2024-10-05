using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Models;
using Db = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.DAL
{
    public static class _DbModelMapping
    {
        public static void RegisterDbMapping(this IServiceCollection services)
        {
            TypeAdapterConfig<Db.Orientation, Orientation>.NewConfig()
                .Map(dest => dest.Id, src => src.OrientationId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.Ethnicity, Ethnicity>.NewConfig()
                .Map(dest => dest.Id, src => src.EthnicityId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.Country, Country>.NewConfig()
                .Map(dest => dest.Id, src => src.CountryId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.TwoLetterCode, src => src.TwoLetterCode)
                .Map(dest => dest.ThreeLetterCode, src => src.ThreeLetterCode)
                .Map(dest => dest.AlternativeNames, src => src.AlternativeNames)
                .TwoWays();

            TypeAdapterConfig<Db.CountryState, CountryState>.NewConfig()
                .Map(dest => dest.Id, src => src.StateId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .TwoWays();

            TypeAdapterConfig<Db.QuizPhrase, QuizPhrase>.NewConfig()
                .Map(dest => dest.Id, src => src.PhraseId)
                .Map(dest => dest.Text, src => src.Text)
                .Map(dest => dest.OrderId, src => src.OrderId)
                .TwoWays();

            TypeAdapterConfig<Db.Unit, Unit>.NewConfig()
                .Map(dest => dest.Id, src => src.UnitId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ConversionRate, src => src.ConversionRate)
                .Map(dest => dest.BaseUnitId, src => src.BaseUnitId)
                .TwoWays();

            TypeAdapterConfig<Db.Unit, UnitShort>.NewConfig()
                .Map(dest => dest.Id, src => src.UnitId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.AccountStatus, AccountStatus>.NewConfig()
                .Map(dest => dest.Id, src => src.StatusId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.AccountType, AccountType>.NewConfig()
                .Map(dest => dest.Id, src => src.AccountTypeId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.MediaFileSource, MediaFileSource>.NewConfig()
                .Map(dest => dest.Id, src => src.SourceId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.MediaFileExtension, MediaFileExtension>.NewConfig()
                .Map(dest => dest.Id, src => src.ExtensionId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsImage, src => src.IsImage)
                .TwoWays();

            TypeAdapterConfig<Db.HalfType, HalfType>.NewConfig()
                .Map(dest => dest.Id, src => src.HalfTypeId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.QuizQuestion, QuizQuestion>.NewConfig()
                .Map(dest => dest.Id, src => src.QuestionId)
                .Map(dest => dest.Value, src => src.Value)
                .TwoWays();

            TypeAdapterConfig<Db.QuizAnswer, QuizAnswer>.NewConfig()
                .Map(dest => dest.Id, src => src.QuizAnswerId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.AskDate, src => src.AskDate)
                .Map(dest => dest.QuestionId, src => src.QuestionId)
                .Map(dest => dest.Question, src => src.Question.Value)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.UnitId, src => src.UnitId)
                .Map(dest => dest.Unit, src => src.Unit.Name)
                .Map(dest => dest.ConvertedValue, src => src.Value * src.Unit.ConversionRate)
                .Map(dest => dest.ConvertedUnit, src => src.Unit.BaseUnit.Name)
                .Map(dest => dest.HalfTypeId, src => src.HalfTypeId)
                .Map(dest => dest.HalfType, src => src.HalfType.Name)
                .Map(dest => dest.Details, src => src.Details);

            TypeAdapterConfig<QuizAnswer, Db.QuizAnswer>.NewConfig()
                .Map(dest => dest.QuizAnswerId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.AskDate, src => src.AskDate)
                .Map(dest => dest.QuestionId, src => src.QuestionId)
                .Ignore(dest => dest.Question)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.UnitId, src => src.UnitId)
                .Ignore(dest => dest.Unit)
                .Map(dest => dest.HalfTypeId, src => src.HalfTypeId)
                .Ignore(dest => dest.HalfType)
                .Map(dest => dest.Details, src => src.Details);

            TypeAdapterConfig<Db.PersonDetail, PersonDetail>.NewConfig()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleName, src => src.MiddleName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.OtherNames, src => src.OtherNames)
                .TwoWays();

            TypeAdapterConfig<Db.PersonLocation, PersonLocation>.NewConfig()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.CountryStateId, src => src.CountryStateId)
                .Map(dest => dest.CountryState, src => src.CountryState.Name)
                .Map(dest => dest.City, src => src.City)
                .Map(dest => dest.Region, src => src.Region)
                .Map(dest => dest.Details, src => src.Details)
                .TwoWays();

            TypeAdapterConfig<Db.Person, Person>.NewConfig()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.EthnicityId, src => src.EthnicityId)
                .Map(dest => dest.Ethnicity, src => src.Ethnicity.Name)
                .Map(dest => dest.OrientationId, src => src.OrientationId)
                .Map(dest => dest.Orientation, src => src.Orientation.Name)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.Country, src => src.Country.Name)
                .Map(dest => dest.Comments, src => src.Comments)
                .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
                .Map(dest => dest.DateOfBirthType, src => (DateOfBirthType)src.DateOfBirthId)
                .Map(dest => dest.Accounts, src => src.Accounts.AsQueryable()
                                                               .OrderBy(a => a.CreatedOn)
                                                               .Adapt<IEnumerable<Account>>())
                .Map(dest => dest.Details, src => src.Details.Adapt<PersonDetail>())
                .Map(dest => dest.Location, src => src.Location.Adapt<PersonLocation>())
                .Map(dest => dest.AnswersCount, src => src.Answers.AsQueryable().Count());

            TypeAdapterConfig<Person, Db.Person>.NewConfig()
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.EthnicityId, src => src.EthnicityId)
                .Ignore(dest => dest.Ethnicity)
                .Map(dest => dest.OrientationId, src => src.OrientationId)
                .Ignore(dest => dest.Orientation)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Ignore(dest => dest.Country)
                .Map(dest => dest.Comments, src => src.Comments)
                .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
                .Map(dest => dest.DateOfBirthId, src => (byte?)src.DateOfBirthType)
                .Ignore(dest => dest.DateOfBirthType)
                .Ignore(dest => dest.Details)
                .Ignore(dest => dest.Location)
                .Ignore(dest => dest.Accounts)
                .Ignore(dest => dest.Answers);

            TypeAdapterConfig<Db.Account, Account>.NewConfig()
                .Map(dest => dest.Id, src => src.AccountId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.StatusId, src => src.StatusId)
                .Map(dest => dest.Status, src => src.Status.Name)
                .Map(dest => dest.AccountTypeId, src => src.AccountTypeId)
                .Map(dest => dest.AccountType, src => src.AccountType.Name)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.DisplayName, src => src.DisplayName)
                .Map(dest => dest.AskDate, src => src.CreatedOn)
                .Map(dest => dest.CreateDate, src => src.CreateDate)
                .Map(dest => dest.LastOnlineDate, src => src.LastOnlineDate)
                .Map(dest => dest.ReconciledDate, src => src.ReconciledDate)
                .Map(dest => dest.Details, src => src.Details);
        }
    }
}
