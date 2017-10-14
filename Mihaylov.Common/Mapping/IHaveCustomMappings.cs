namespace Mihaylov.Common.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
       void CreateMappings(IMapperConfigurationExpression configuration);
    }
}