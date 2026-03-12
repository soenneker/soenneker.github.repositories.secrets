using Soenneker.GitHub.OpenApiClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GitHub.Repositories.Secrets.Abstract;

/// <summary>
/// Provides utilities for managing GitHub repository secrets, including reading, writing, and deleting secrets.
/// </summary>
public interface IGitHubRepositoriesSecretsUtil
{
    /// <summary>
    /// Retrieves all secrets for a given repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>A list of secrets defined in the repository.</returns>
    ValueTask<List<ActionsSecret>> GetAll(string owner, string repo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all organization-level secrets accessible by a given repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>A list of organization secrets available to the repository.</returns>
    ValueTask<List<ActionsSecret>> GetOrganization(string owner, string repo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific secret from a given repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="name">The name of the secret.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>The secret object if found.</returns>
    ValueTask<ActionsSecret> GetAll(string owner, string repo, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the public key for encrypting secrets in a repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>A tuple containing the key ID and base64-encoded public key.</returns>
    ValueTask<(string KeyId, string Key)> GetPublicKey(string owner, string repo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or updates a secret in a repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="name">The name of the secret to create or update.</param>
    /// <param name="value">The plaintext value of the secret to be encrypted and stored.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    ValueTask CreateOrUpdate(string owner, string repo, string name, string value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a specific secret from a repository.
    /// </summary>
    /// <param name="owner">The owner of the repository (user or organization).</param>
    /// <param name="repo">The name of the repository.</param>
    /// <param name="name">The name of the secret to delete.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    ValueTask Delete(string owner, string repo, string name, CancellationToken cancellationToken = default);
}