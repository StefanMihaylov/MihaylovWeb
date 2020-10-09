namespace Mihaylov.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public static class MapperExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(this IQueryable source, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return source.ProjectTo(AutoMapperConfigurator.Configuration, membersToExpand);
        }

        public static TDestination ToModel<TDestination>(this object source)
        {
           return Mapper.Map<TDestination>(source);
        }
    }
}
