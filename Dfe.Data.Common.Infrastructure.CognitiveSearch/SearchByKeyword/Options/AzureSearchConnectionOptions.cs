using System.ComponentModel.DataAnnotations;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

/// <summary>
/// Configuration options used to define the internal Azure
/// cognitive search service credentials and Uri endpoint.
/// </summary>
public sealed class AzureSearchConnectionOptions
{
    /// <summary>
    /// The credentials used to access the cognitive search service.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? Credentials { get; set; }

    /// <summary>
    /// The Uri of the cognitive search service endpoint used.
    /// </summary>
    [Url]
    [Required(AllowEmptyStrings = false)]
    public string? EndpointUri { get; set; }
}
