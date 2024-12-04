using Soenneker.GitHub.Repositories.Secrets.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.GitHub.Repositories.Secrets.Tests;

[Collection("Collection")]
public class GitHubRepositoriesSecretsUtilTests : FixturedUnitTest
{
    private readonly IGitHubRepositoriesSecretsUtil _util;

    public GitHubRepositoriesSecretsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGitHubRepositoriesSecretsUtil>(true);
    }
}
