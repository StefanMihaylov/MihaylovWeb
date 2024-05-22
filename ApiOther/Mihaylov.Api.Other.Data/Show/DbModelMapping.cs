using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Dbr = Mihaylov.Api.Other.Data.Show.Repositories;
using Db = Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Data.Show
{
    public static class DbModelMapping
    {
        public static void RegisterDbMapping(this IServiceCollection services)
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
    }
}
