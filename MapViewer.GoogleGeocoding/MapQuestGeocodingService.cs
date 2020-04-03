using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Geocoding;
using Geocoding.Google;
using Geocoding.MapQuest;
using Geographics;
using MapViewer.Geocoding;

namespace MapViewer.GoogleGeocoding
{
    public class MapQuestGeocodingService : IGeocodingService
    {

        private static readonly SemaphoreSlim _googleSemaphore = new SemaphoreSlim(5);

        private readonly MapQuestGeocoder _okGoogle;

        private DateTime _lastAccessTime;
        private readonly TimeSpan _minimumInterval = TimeSpan.FromMilliseconds(50);

        public MapQuestGeocodingService()
        {
            _okGoogle  = new MapQuestGeocoder("DsDPrdlxAkocCwOLzAvZNk83v7qcQRru");

            _lastAccessTime = DateTime.Today;
        }

        public async Task<string> GetPlacementName(EarthPoint Point, CancellationToken Cancellation)
        {
            Exception exception = null;
            for (var i = 0; i < 5; i++)
            {
                await _googleSemaphore.WaitAsync(Cancellation).ConfigureAwait(false);
                try
                {
                    var waitFor = _lastAccessTime + _minimumInterval - DateTime.Now;
                    if (waitFor.TotalSeconds > 0)
                        await Task.Delay(waitFor, Cancellation);

                    Cancellation.ThrowIfCancellationRequested();
                    
                    var geocode = await _okGoogle.ReverseGeocodeAsync(Point.Latitude.Value, Point.Longitude.Value)
                                                 .ConfigureAwait(false);

                    var address = geocode.OfType<ParsedAddress>().FirstOrDefault();
                    if (address == null)
                        return string.Empty;

                    return string.Join(", ", new [] {address.City, address.State}.Where(a => !string.IsNullOrWhiteSpace(a)));
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