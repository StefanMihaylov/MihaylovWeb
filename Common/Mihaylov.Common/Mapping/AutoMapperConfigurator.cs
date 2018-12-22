namespace Mihaylov.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    public class AutoMapperConfigurator
    {
        private ICollection<Assembly> assemblies;

        public static MapperConfiguration Configuration { get; private set; }

        public static IMapper Mapper
        {
            get
            {
                return Configuration.CreateMapper();
            }
        }

        public AutoMapperConfigurator(Assembly currentAssembly, params string[] assemblies)
        {
            this.assemblies = new List<Assembly>();

            if (currentAssembly != null)
            {
                this.assemblies.Add(currentAssembly);
            }

            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var assemblyName in assemblies)
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(a => a.GetName().Name.Equals(assemblyName));

                    if (assembly != null)
                    {
                        this.assemblies.Add(assembly);
                    }
                }
            }
        }

        public void Execute()
        {
            Configuration = new MapperConfiguration(
                configuration =>
                {
                    List<Type> types = new List<Type>();
                    foreach (var assembly in this.assemblies)
                    {
                        types.AddRange(assembly.GetExportedTypes());
                    }

                    LoadStandardMappings(types, configuration);
                    LoadReverseMappings(types, configuration);
                    LoadCustomMappings(types, configuration);
                });
        }

        private void LoadStandardMappings(IEnumerable<Type> allTypes, IMapperConfigurationExpression mapperConfiguration)
        {
            var maps = allTypes.SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                                .Where(v => !v.Type.IsAbstract)
                                .Where(v => !v.Type.IsInterface)
                                .Where(v => v.Interface.IsGenericType)
                                .Where(v => v.Interface.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                                .Select(v => new
                                {
                                    Source = v.Interface.GetGenericArguments()[0],
                                    Destination = v.Type
                                })
                                .ToArray();

            foreach (var map in maps)
            {
                mapperConfiguration.CreateMap(map.Source, map.Destination);
            }
        }

        private void LoadReverseMappings(IEnumerable<Type> allTypes, IMapperConfigurationExpression mapperConfiguration)
        {
            var maps = (from t in allTypes
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = t,
                            Destination = i.GetGenericArguments()[0],
                        })
                        .ToArray();

            foreach (var map in maps)
            {
                mapperConfiguration.CreateMap(map.Source, map.Destination);
            }
        }

        private void LoadCustomMappings(IEnumerable<Type> allTypes, IMapperConfigurationExpression mapperConfiguration)
        {
            var types = (from t in allTypes
                         from i in t.GetInterfaces()
                         where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                               !t.IsAbstract &&
                               !t.IsInterface
                         select t)
                        .ToArray();

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            foreach (var type in types)
            {
                var instance = (IHaveCustomMappings)Activator.CreateInstance(type, flags, null, null, null);
                instance.CreateMappings(mapperConfiguration);
            }
        }
    }
}
