using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GitHub.Repositories.Secrets.Abstract;

/// <summary>
/// A utility library for GitHub Repository Secret related operations
/// </summary>
public interface IGitHubRepositoriesSecretsUtil
{
    ValueTask SetSecret(string repository, string gitHubUsername, string secretName, string secretValue, bool log = true, CancellationToken cancellationToken = default);
}