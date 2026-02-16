using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.IndexNames;
public sealed class AzureConfigurationSearchIndexNamesProviderTests
{
    [Fact]
    public void Constructor_Throws_When_OptionsIsNull()
    {
        // Arrange
        SearchIndexNamesOptions? options = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AzureConfigurationSearchIndexNamesProvider(options!));
    }

    [Fact]
    public void Constructor_Throws_When_Options_Names_Is_Null()
    {
        // Arrange
        SearchIndexNamesOptions? options = new()
        {
            Names = null
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AzureConfigurationSearchIndexNamesProvider(options!));
    }

    [Fact]
    public void Constructor_Throws_When_Options_Names_Has_NoValues()
    {
        // Arrange
        SearchIndexNamesOptions? options = new()
        {
            Names = []
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AzureConfigurationSearchIndexNamesProvider(options!));
    }

    [Fact]
    public void GetIndexNames_ReturnsExpectedIndexNames()
    {
        // Arrange
        List<string> expectedIndexNames = ["index-1", "INDEX-2", " Index-3 "];

        SearchIndexNamesOptions options = new()
        {
            Names = expectedIndexNames
        };

        AzureConfigurationSearchIndexNamesProvider provider = new(options);

        // Act
        IEnumerable<string> actualIndexNames = provider.GetIndexNames();

        // Assert
        Assert.NotNull(actualIndexNames);
        Assert.Equal(expectedIndexNames.Count, actualIndexNames.Count());
        Assert.All(expectedIndexNames, indexName => Assert.Contains(indexName, actualIndexNames));
    }
}
