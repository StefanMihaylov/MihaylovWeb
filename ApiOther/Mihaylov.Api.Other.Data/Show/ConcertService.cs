using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Data.Show
{
    public class ConcertService : IConcertService
    {
        private readonly ILogger _logger;
        private readonly IBandRepository _bands;
        private readonly IConcertRepository _concerts;
        private readonly ILocationRepository _locations;
        private readonly ITicketProviderRepository _provider;
        private readonly IMemoryCache _memoryCache;

        private const string SHOW_ALL_BANDS = "show_all_bands";
        private const string SHOW_ALL_LOCATION = "show_all_Location";
        private const string SHOW_ALL_TICKETPROVIDER = "show_all_TicketProvider";
        private const int CACHE_DURATION = 30;

        public ConcertService(ILoggerFactory loggerFactory, IBandRepository bands, IConcertRepository concerts,
            ILocationRepository locations, ITicketProviderRepository provider, IMemoryCache memoryCache)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _bands = bands;
            _concerts = concerts;
            _locations = locations;
            _provider = provider;
            _memoryCache = memoryCache;
        }

        public async Task<Grid<ConcertExtended>> GetConcertsAsync(GridRequest request)
        {
            var concerts = await _concerts.GetAllAsync(request).ConfigureAwait(false);

            return concerts;
        }

        public async Task<ConcertExtended> AddOrUpdateConcertAsync(Concert model)
        {
            var concert = await _concerts.AddOrUpdateAsync(model).ConfigureAwait(false);

            return concert;
        }

        public async Task<Grid<BandExtended>> GetBandsAsync(GridRequest request)
        {
            var key = $"{SHOW_ALL_BANDS}_{request.Page}";
            if (!_memoryCache.TryGetValue(key, out Grid<BandExtended> bands))
            {
                bands = await _bands.GetAllAsync(request).ConfigureAwait(false);

                _memoryCache.Set(key, bands, TimeSpan.FromMinutes(CACHE_DURATION));
                AddCacheKeys(SHOW_ALL_BANDS, key);
            }

            return bands;
        }

        public async Task<Band> AddOrUpdateBandAsync(Band model)
        {

            var band = await _bands.AddOrUpdateAsync(model).ConfigureAwait(false);
            ClearCache(SHOW_ALL_BANDS);

            return band;
        }

        public async Task<Grid<Location>> GetLocationsAsync(GridRequest request)
        {
            var key = $"{SHOW_ALL_LOCATION}_{request.Page}";
            if (!_memoryCache.TryGetValue(key, out Grid<Location> locations))
            {
                locations = await _locations.GetAllAsync(request).ConfigureAwait(false);

                _memoryCache.Set(key, locations, TimeSpan.FromMinutes(CACHE_DURATION));
                AddCacheKeys(SHOW_ALL_LOCATION, key);
            }

            return locations;
        }

        public async Task<Location> AddOrUpdateLocationAsync(Location model)
        {
            var location = await _locations.AddOrUpdateAsync(model).ConfigureAwait(false);
            ClearCache(SHOW_ALL_LOCATION);

            return location;
        }

        public async Task<Grid<TicketProvider>> GetTicketProvidersAsync(GridRequest request)
        {
            var key = $"{SHOW_ALL_TICKETPROVIDER}_{request.Page}";
            if (!_memoryCache.TryGetValue(key, out Grid<TicketProvider> providers))
            {
                providers = await _provider.GetAllAsync(request).ConfigureAwait(false);

                _memoryCache.Set(key, providers, TimeSpan.FromMinutes(CACHE_DURATION));
                AddCacheKeys(SHOW_ALL_TICKETPROVIDER, key);
            }

            return providers;
        }

        public async Task<TicketProvider> AddOrUpdateTicketProviderAsync(TicketProvider model)
        {
            var provider = await _provider.AddOrUpdateAsync(model).ConfigureAwait(false);
            ClearCache(SHOW_ALL_TICKETPROVIDER);

            return provider;
        }

        private void ClearCache(string key)
        {
            if (_memoryCache.TryGetValue(key, out List<string> cacheKeys))
            {
                foreach (var cacheKey in cacheKeys)
                {
                    _memoryCache.Remove(cacheKey);
                }
            }
        }

        private void AddCacheKeys(string mainKey, string pageKey)
        {
            if (!_memoryCache.TryGetValue(mainKey, out List<string> cacheKeys))
            {
                cacheKeys = new List<string>();
            }

            cacheKeys.Add(pageKey);

            _memoryCache.Set(mainKey, cacheKeys, TimeSpan.FromMinutes(CACHE_DURATION));
        }
    }
}
