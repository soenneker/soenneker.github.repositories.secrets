using Soenneker.GitHub.Repositories.Secrets.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.GitHub.Repositories.Secrets.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class GitHubRepositoriesSecretsUtilTests : HostedUnitTest
{
    private readonly IGitHubRepositoriesSecretsUtil _util;

    public GitHubRepositoriesSecretsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IGitHubRepositoriesSecretsUtil>(true);
    }

    [Test]
    public void Default()
    { }
}
