using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Cluster.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;
using DbC = Mihaylov.Api.Other.Database.Cluster.Models;
using Db = Mihaylov.Api.Other.Database.Shows.Models;
using Dbr = Mihaylov.Api.Other.DAL.Show;

namespace Mihaylov.Api.Other.DAL
{
    public static class _DbModelMapping
    {
        public static void RegisterDbMapping(this IServiceCollection services)
        {
            RegisterShow();
            RegisterCluster();
        }

        private static void RegisterShow()
        {
            TypeAdapterConfig<Db.Band, Band>.NewConfig()
                .Map(dest => dest.Id, src => src.BandId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Dbr.DbBandExt, BandExtended>.NewConfig()
                .Map(dest => dest.Id, src => src.Band.BandId)
                .Map(dest => dest.Name, src => src.Band.Name)
                .Map(dest => dest.Count, src => src.Count);

            TypeAdapterConfig<Db.Location, Location>.NewConfig()
                .Map(dest => dest.Id, src => src.LocationId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<Db.TicketProvider, TicketProvider>.NewConfig()
                .Map(dest => dest.Id, src => src.TickerProviderId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Url, src => src.Url)
                .TwoWays();

            TypeAdapterConfig<Db.Concert, ConcertExtended>.NewConfig()
                .Map(dest => dest.Id, src => src.ConcertId)
                .Map(dest => dest.Date, src => src.Date)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.LocationId, src => src.LocationId)
                .Map(dest => dest.Location, src => src.Location.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Currency, src => (CurrencyType)src.CurrencyId)
                .Map(dest => dest.TicketProviderId, src => src.TicketProviderId)
                .Map(dest => dest.TicketProvider, src => src.TicketProvider.Name)
                .Map(dest => dest.Bands, src => src.ConcertBands.AsQueryable()
                                                        .OrderBy(cb => cb.Order)
                                                        .Select(cb => cb.Band)
                                                        .Adapt<IEnumerable<Band>>());

            TypeAdapterConfig<Concert, Db.Concert>.NewConfig()
                .Map(dest => dest.ConcertId, src => src.Id)
                .Map(dest => dest.Date, src => src.Date)
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.LocationId, src => src.LocationId)
                .Ignore(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CurrencyId, src => (byte)src.Currency)
                .Ignore(dest => dest.Currency, src => src.Currency)
                .Map(dest => dest.TicketProviderId, src => src.TicketProviderId)
                .Ignore(dest => dest.TicketProvider, src => src.TicketProvider)
                .Ignore(dest => dest.Bands, src => src.Bands)
                .Ignore(dest => dest.ConcertBands, src => src.ConcertBands);
        }

        private static void RegisterCluster()
        {
            TypeAdapterConfig<DbC.Application, ApplicationExtended>.NewConfig()
                .Map(dest => dest.Id, src => src.ApplicationId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.SiteUrl, src => src.SiteUrl)
                .Map(dest => dest.ReleaseUrl, src => src.ReleaseUrl)
                .Map(dest => dest.GithubVersionUrl, src => src.GithubVersionUrl)
                .Map(dest => dest.ResourceUrl, src => src.ResourceUrl)
                .Map(dest => dest.Deployment, src => (DeploymentType)src.DeploymentId)
                .Map(dest => dest.Files, src => src.Files.AsQueryable()
                                                .OrderBy(f => f.FileId)
                                                .Adapt<IEnumerable<DeploymentFile>>())
                .Map(dest => dest.Pods, src => src.Pods.AsQueryable()
                                                .OrderBy(f => f.ApplicationPodId)
                                                .Adapt<IEnumerable<Pod>>())
                .Map(dest => dest.Version, src => src.Versions.AsQueryable()
                                                .OrderByDescending(f => f.Version)
                                                .Adapt<IEnumerable<AppVersion>>()
                                                .FirstOrDefault())
                .Map(dest => dest.Notes, src => src.Notes)
                .Map(dest => dest.Order, src => src.Order);

            TypeAdapterConfig<Application, DbC.Application>.NewConfig()
                .Map(dest => dest.ApplicationId, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.SiteUrl, src => src.SiteUrl)
                .Map(dest => dest.ReleaseUrl, src => src.ReleaseUrl)
                .Map(dest => dest.GithubVersionUrl, src => src.GithubVersionUrl)
                .Map(dest => dest.ResourceUrl, src => src.ResourceUrl)
                .Map(dest => dest.DeploymentId, src => (byte)src.Deployment)
                .Ignore(dest => dest.Deployment, src => src.Deployment)
                .Ignore(dest => dest.Files, src => src.Files)
                .Ignore(dest => dest.Pods, src => src.Pods)
                .Ignore(dest => dest.Versions, src => src.Versions)
                .Map(dest => dest.Notes, src => src.Notes)
                .Map(dest => dest.Order, src => src.Order);

            TypeAdapterConfig<DbC.ApplicationPod, Pod>.NewConfig()
                .Map(dest => dest.Id, src => src.ApplicationPodId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<DbC.DeploymentFile, DeploymentFile>.NewConfig()
                .Map(dest => dest.Id, src => src.FileId)
                .Map(dest => dest.Name, src => src.Name)
                .TwoWays();

            TypeAdapterConfig<DbC.ApplicationVersion, AppVersion>.NewConfig()
                .Map(dest => dest.Id, src => src.VersionId)
                .Map(dest => dest.Version, src => src.Version)
                .Map(dest => dest.HelmVersion, src => src.HelmVersion)
                .Map(dest => dest.HelmAppVersion, src => src.HelmAppVersion)
                .Map(dest => dest.ReleaseDate, src => src.ReleaseDate)
                .TwoWays();

            TypeAdapterConfig<ParserSetting, DbC.ParserSetting>.NewConfig()
                .Map(dest => dest.ParserSettingId, src => src.Id)
                .Map(dest => dest.ApplicationId, src => src.ApplicationId)
                .Ignore(dest => dest.Application, src => src.Application)
                .Map(dest => dest.VersionUrlVersionId, src => (byte)src.VersionUrlType)
                .Ignore(dest => dest.VersionUrlVersion, src => src.VersionUrlVersion)
                .Map(dest => dest.SelectorVersion, src => src.VersionSelector)
                .Map(dest => dest.CommandsVersion, src => src.VersionCommand)
                .Map(dest => dest.VersionUrlrReleaseId, src => (byte?)src.ReleaseDateUrlType)
                .Ignore(dest => dest.VersionUrlrRelease, src => src.VersionUrlrRelease)
                .Map(dest => dest.SelectorRelease, src => src.ReleaseDateSelector)
                .Map(dest => dest.CommandsRelease, src => src.ReleaseDateCommand);

            TypeAdapterConfig<DbC.ParserSetting, ParserSetting>.NewConfig()
                .Map(dest => dest.Id, src => src.ParserSettingId)
                .Map(dest => dest.ApplicationId, src => src.ApplicationId)
                .Map(dest => dest.ApplicationName, src => src.Application.Name)
                .Map(dest => dest.VersionUrlType, src => (VersionUrlType)src.VersionUrlVersionId)
                .Map(dest => dest.VersionSelector, src => src.SelectorVersion)
                .Map(dest => dest.VersionCommand, src => src.CommandsVersion)
                .Map(dest => dest.ReleaseDateUrlType, src => (VersionUrlType?)src.VersionUrlrReleaseId)
                .Map(dest => dest.ReleaseDateSelector, src => src.SelectorRelease)
                .Map(dest => dest.ReleaseDateCommand, src => src.CommandsRelease);
        }
    }
}
