using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.TestDoubles.StubBuilders;

internal class SearchByKeywordClientOptionsBuilder
{
    private string _credentials = "fhjk67=j#l";
    private string _endpointUri = Faker.Internet.SecureUrl();

    public AzureSearchConnectionOptions Create() =>
        new AzureSearchConnectionOptions()
        {
            Credentials = _credentials,
            EndpointUri = _endpointUri,
        };

    public SearchByKeywordClientOptionsBuilder WithInvalidCredentials()
    {
        _credentials = "";
        return this;
    }
    public SearchByKeywordClientOptionsBuilder WithInvalidEndpoint()
    {
        _endpointUri = "";
        return this;
    }
}
