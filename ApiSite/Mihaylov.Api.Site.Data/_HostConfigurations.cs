using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Helpers;
using Mihaylov.Api.Site.Data.Managers;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Api.Site.Data.Media.Services;
using Mihaylov.Api.Site.Data.Models;
using Mihaylov.Api.Site.Data.Writers;
using Mihaylov.Common.Generic.Servises;
using Mihaylov.Common.Generic.Servises.Interfaces;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddSiteServices(this IServiceCollection services, Action<SiteOptions> siteOption)
        {
            services.Configure<SiteOptions>(siteOption);

            services.AddScoped<ICollectionManager, CollectionManager>();
            services.AddScoped<IPersonsManager, PersonsManager>();
            services.AddScoped<IQuizManager, QuizManager>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();

            services.AddScoped<ICollectionWriter, CollectionWriter>();
            services.AddScoped<IPersonsWriter, PersonsWriter>();
            services.AddScoped<IQuizWriter, QuizWriter>();
            services.AddScoped<IConfigurationWriter, ConfigurationWriter>();

            services.AddScoped<ISiteHelper, SiteHelper>();

            services.AddHttpClient("SiteHelper")
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler()
                        {
                            AllowAutoRedirect = false
                        };
                    });

            return services;
        }

        public static IServiceCollection AddMediaServices(this IServiceCollection services, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetExecutingAssembly();

            var fileInfo = new FileInfo(assembly.Location);
            var directory = fileInfo.DirectoryName;

            services.Configure<PathConfig>(opt =>
            {
                opt.BasePath = directory;
                opt.FFMpegPath = Path.Combine(directory, "Media/binaries");
                opt.TempPath = Path.Combine(directory, "temp");
            });

            services.TryAddScoped<IFileWrapper, FileWrapper>();
            services.AddScoped<IImageMediaService, ImageMediaService>();
            services.AddScoped<IVideoMediaService, VideoMediaService>();
            services.AddScoped<IMediaService, MediaService>();

            services.AddScoped<IImageHash, PerceptualHash>(); // AverageHash, DifferenceHash,

            return services;
        }
    }
}
