using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Db = Mihaylov.Api.Dictionary.Database.Models;

namespace Mihaylov.Api.Dictionary.DAL
{
    public static class _DbModelMapping
    {
        public static void RegisterDbMapping(this IServiceCollection services)
        {
            TypeAdapterConfig<Db.Course, Course>.NewConfig()
                .Map(dest => dest.Id, src => src.CourseId)
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.EndDate, src => src.EndDate)
                .Map(dest => dest.Level, src => src.Level.Name)
                .Map(dest => dest.ModulesStartNumber, src => src.ModulesStartNumber)
                .Map(dest => dest.ModulesEndNumber, src => src.ModulesEndNumber)
                .Map(dest => dest.LearningSystemId, src => src.LearningSystemId)
                .Map(dest => dest.LearningSystem, src => src.LearningSystem.Name)
                .Map(dest => dest.Language, src => src.LearningSystem.Language.Name);

            TypeAdapterConfig<Course, Db.Course>.NewConfig()
                .Map(dest => dest.CourseId, src => src.Id)
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.EndDate, src => src.EndDate)
                .Ignore(dest => dest.Level)
                .Map(dest => dest.ModulesStartNumber, src => src.ModulesStartNumber)
                .Map(dest => dest.ModulesEndNumber, src => src.ModulesEndNumber)
                .Map(dest => dest.LearningSystemId, src => src.LearningSystemId)
                .Ignore(dest => dest.LearningSystem);

            TypeAdapterConfig<Db.Language, Language>.NewConfig()
                .Map(dest => dest.Id, src => src.LanguageId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.LearningSystem, LearningSystem>.NewConfig()
                .Map(dest => dest.Id, src => src.LearningSystemId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.LanguageId, src => src.LanguageId)
                .Map(dest => dest.Language, src => src.Language.Name);

            TypeAdapterConfig<LearningSystem, Db.LearningSystem>.NewConfig()
                .Map(dest => dest.LearningSystemId, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.LanguageId, src => src.LanguageId)
                .Ignore(dest => dest.Language);

            TypeAdapterConfig<Db.Level, Level>.NewConfig()
                .Map(dest => dest.Id, src => src.LevelId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Descrition, src => src.Descrition)
                .TwoWays();

            TypeAdapterConfig<Db.RecordType, RecordType>.NewConfig()
                .Map(dest => dest.Id, src => src.RecordTypeId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.Preposition, Preposition>.NewConfig()
                .Map(dest => dest.Id, src => src.PrepositionId)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.LanguageId, src => src.LanguageId)
                .Map(dest => dest.Language, src => src.Language.Name);

            TypeAdapterConfig<Preposition, Db.Preposition>.NewConfig()
                .Map(dest => dest.PrepositionId, src => src.Id)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.LanguageId, src => src.LanguageId)
                .Ignore(dest => dest.Language);

            TypeAdapterConfig<Db.Record, Record>.NewConfig()
                .Map(dest => dest.Id, src => src.RecordId)
                .Map(dest => dest.CourseId, src => src.CourseId)
                .Map(dest => dest.ModuleNumber, src => src.ModuleNumber)
                .Map(dest => dest.Original, src => src.Original)
                .Map(dest => dest.Translation, src => src.Translation)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.RecordTypeId, src => src.RecordTypeId)
                .Map(dest => dest.RecordType, src => src.RecordType.Name)
                .Map(dest => dest.PrepositionId, src => src.PrepositionId)
                .Map(dest => dest.Preposition, src => src.Preposition.Value);

            TypeAdapterConfig<Record, Db.Record>.NewConfig()
                .Map(dest => dest.RecordId, src => src.Id)
                .Map(dest => dest.CourseId, src => src.CourseId)
                .Ignore(dest => dest.Course)
                .Map(dest => dest.ModuleNumber, src => src.ModuleNumber)
                .Map(dest => dest.Original, src => src.Original)
                .Map(dest => dest.Translation, src => src.Translation)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.RecordTypeId, src => src.RecordTypeId)
                .Ignore(dest => dest.RecordType)
                .Map(dest => dest.PrepositionId, src => src.PrepositionId)
                .Ignore(dest => dest.Preposition);
        }
    }
}
