using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Model;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Providers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation;

/// <summary>
/// The default service provisioned to allow requests to made to the azure location services
/// provide a consistent, configured response based on the status of the request work-flow.
/// </summary>
public sealed class DefaultGeoLocationService : IGeoLocationService
{
    private readonly IGeoLocationClientProvider _geoLocationClientProvider;
    private readonly GeoLocationOptions _geoLocationOptions;

    /// <summary>
    /// The following dependencies include the geo-location client provider, and the geo-location options required to 
    /// the complete implementation of which is defined in the IOC container.
    /// </summary>
    /// <param name="geoLocationClientProvider">
    /// Provides a readily configured HttpClient (setup and registered in the native DI container)
    /// for use when making geo-location service requests.
    /// </param>
    /// <param name="geoLocationOptions">
    /// Configuration options used to define the properties required to make a successful geo-location search request.
    /// </param>
    public DefaultGeoLocationService(
        IGeoLocationClientProvider geoLocationClientProvider,
        IOptions<GeoLocationOptions> geoLocationOptions)
    {
        ArgumentNullException.ThrowIfNull(geoLocationOptions);
        ArgumentNullException.ThrowIfNull(geoLocationClientProvider);

        _geoLocationClientProvider = geoLocationClientProvider;
        _geoLocationOptions = geoLocationOptions.Value;
    }

    /// <summary>
    /// Makes a call to the underlying azure location service and uses the prescribed HttpClient to
    /// manage the request and aggregate the result(s) based on the status of the response.
    /// </summary>
    /// <param name="location">
    /// The location string on which to establish the basis of the search.
    /// </param>
    /// <returns>
    /// A configured T:Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Model.GeoLocationServiceResponse instance.
    /// </returns>
    /// <exception cref="JsonException">
    /// Exception thrown if a success status is returned with an invalid JSON response.
    /// </exception>
    public Task<GeoLocationServiceResponse> SearchGeoLocationAsync(string location)
    {
        ArgumentException.ThrowIfNullOrEmpty(location);
        ArgumentException.ThrowIfNullOrWhiteSpace(_geoLocationOptions.SearchEndpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(_geoLocationOptions.MapsSubscriptionKey);

        return _geoLocationClientProvider.InvokeGeoLocationClientAsync().ContinueWith(
            async geoLocationClient =>
            {
                using var geoLocationResponse =
                      await geoLocationClient.Result.GetAsync(
                          string.Format(
                              _geoLocationOptions.SearchEndpoint, location,
                              _geoLocationOptions.MapsSubscriptionKey),
                        HttpCompletionOption.ResponseHeadersRead)
                      .ConfigureAwait(false);

                geoLocationResponse.EnsureSuccessStatusCode();

                string rawGeoLocationResponse =
                    await geoLocationResponse.Content
                        .ReadAsStringAsync().ConfigureAwait(false);

                return JsonSerializer
                    .Deserialize<GeoLocationServiceResponse>(rawGeoLocationResponse) ??
                    throw new JsonException(
                        $"Unable to de-serialise the geo-location response: {rawGeoLocationResponse}");
            },
            TaskContinuationOptions.OnlyOnRanToCompletion).Result;
    }
}
