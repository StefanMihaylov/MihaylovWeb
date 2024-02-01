using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;
using Mihaylov.Site.Media.Services;

namespace Mihaylov.Site.Media
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddMediaServices(this IServiceCollection services, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetExecutingAssembly();

            var fileInfo = new FileInfo(assembly.Location);
            var directory = fileInfo.DirectoryName;

            services.Configure<FileWrapperConfig>(opt =>
            {
                opt.BasePath = directory;
                opt.FFMpegPath = Path.Combine(directory, "binaries");
                opt.TempPath = Path.Combine(directory, "temp");
            });

            services.AddScoped<IFileWrapper, FileWrapper>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IImageMediaService, ImageMediaService>();
            services.AddScoped<IVideoMediaService, VideoMediaService>();

            return services;
        }

        public static int GetPersentage(this long processed, long total)
        {
            return (int)((decimal)processed / total * 100);
        }
    }
}
