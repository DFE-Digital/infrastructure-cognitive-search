using System.ComponentModel.DataAnnotations;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Options;

/// <summary>
///  Configuration options used to define the properties required to make a successful geo-location search request.
/// </summary>
public sealed class GeoLocationOptions
{
    /// <summary>
    /// The Uri of the azure mapping service used for geo-location searches.
    /// </summary>
    [Url]
    [Required]
    public string? MapsServiceUri { get; set; }

    /// <summary>
    /// The Uri of the azure mapping service endpoint used for geo-location searches.
    /// </summary>
    [Required]
    public string? SearchEndpoint { get; set; }

    /// <summary>
    /// The subscription key of the azure mapping service used for geo-location searches.
    /// </summary>
    [Required]
    public string? MapsSubscriptionKey { get; set; }

    /// <summary>
    /// The HttpClient (associated with the geo-location search) request  timeout in hours.
    /// </summary>
    public int RequestTimeOutHours { get; set; }

    /// <summary>
    /// The HttpClient (associated with the geo-location search) request  timeout in minutes.
    /// </summary>
    public int RequestTimeOutMinutes { get; set; }

    /// <summary>
    /// The HttpClient (associated with the geo-location search) request  timeout in seconds.
    /// </summary>
    public int RequestTimeOutSeconds { get; set; }
}
