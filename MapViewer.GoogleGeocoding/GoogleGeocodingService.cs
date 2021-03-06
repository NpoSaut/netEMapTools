﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Geocoding.Google;
using Geographics;
using MapViewer.Geocoding;

namespace MapViewer.GoogleGeocoding
{
    public class GoogleGeocodingService : IGeocodingService, IDisposable
    {
        private static readonly GoogleAddressType[] _goodTypes =
        {
            GoogleAddressType.AdministrativeAreaLevel2,
            GoogleAddressType.AdministrativeAreaLevel1,
            GoogleAddressType.Locality
        };

        private static readonly SemaphoreSlim _googleSemaphore = new SemaphoreSlim(5);

        private readonly GoogleGeocoder _okGoogle;
        private readonly WebClient _webClient;
        
        private DateTime _lastAccessTime;
        private TimeSpan _minimumInterval = TimeSpan.FromMilliseconds(750);

        public GoogleGeocodingService()
        {
            _webClient = new WebClient { Encoding = Encoding.UTF8 };
            _okGoogle = new GoogleGeocoder { Language = "ru" };

            _lastAccessTime = DateTime.Today;
        }

        public void Dispose() { _webClient.Dispose(); }

        public async Task<string> GetPlacementName(EarthPoint Point, CancellationToken token)
        {
            Exception exception = null;
            for (int i = 0; i < 5; i++)
            {
                await _googleSemaphore.WaitAsync().ConfigureAwait(false);

                var waitFor = (_lastAccessTime + _minimumInterval) - DateTime.Now;
                if (waitFor.TotalSeconds > 0)
                    await Task.Delay(waitFor);
                
                try
                {
                    IEnumerable<GoogleAddress> geocode = await _okGoogle.ReverseGeocodeAsync(Point.Latitude.Value, Point.Longitude.Value).ConfigureAwait(false);
                    GoogleAddress address = geocode.Last(a => a.Components.Any(c => c.Types.Contains(GoogleAddressType.AdministrativeAreaLevel2)));
                    IEnumerable<GoogleAddressComponent> addressComponents = address.Components.Where(c => c.Types.Any(t => _goodTypes.Contains(t)));
                    return string.Join(", ", addressComponents.Select(c => c.ShortName));
                }
                catch (Exception e)
                {
                    exception = e;
                }
                finally
                {
                    _lastAccessTime = DateTime.Now;
                    _googleSemaphore.Release();
                }
            }
            throw exception;
        }
    }
}
