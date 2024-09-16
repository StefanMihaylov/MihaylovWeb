using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Models;
using Db = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.Data
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
        }
    }
}
