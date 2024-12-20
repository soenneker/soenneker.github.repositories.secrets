using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GitHub.Repositories.Secrets.Abstract;

/// <summary>
/// A utility library for GitHub Repository Secret related operations
/// </summary>
public interface IGitHubRepositoriesSecretsUtil
{
    /// <summary>
    /// Creates or updates a secret in the specified repository.
    /// </summary>
    /// <param name="owner">The GitHub username associated with the repository.</param>
    /// <param name="name">The name of the repository where the secret will be stored.</param>
    /// <param name="secretName">The name of the secret to create or update.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <param name="log">Indicates whether to log the operation. Defaults to <c>true</c>.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Upsert(string owner, string name, string secretName, string secretValue, bool log = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a secret from the specified repository.
    /// </summary>
    /// <param name="owner">The owner of the repository where the secret is stored.</param>
    /// <param name="name">The name of the repository.</param>
    /// <param name="secretName">The name of the secret to delete.</param>
    /// <param name="log">Indicates whether to log the operation. Defaults to <c>true</c>.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Delete(string owner, string name, string secretName, bool log = true, CancellationToken cancellationToken = default);
}