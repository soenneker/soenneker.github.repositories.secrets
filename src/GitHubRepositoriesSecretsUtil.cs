using Microsoft.Extensions.Logging;
using Sodium;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.GitHub.ClientUtil.Abstract;
using Soenneker.GitHub.OpenApiClient;
using Soenneker.GitHub.OpenApiClient.Models;
using Soenneker.GitHub.OpenApiClient.Repos.Item.Item.Actions.OrganizationSecrets;
using Soenneker.GitHub.OpenApiClient.Repos.Item.Item.Actions.Secrets;
using Soenneker.GitHub.OpenApiClient.Repos.Item.Item.Actions.Secrets.Item;
using Soenneker.GitHub.Repositories.Secrets.Abstract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GitHub.Repositories.Secrets;

/// <summary>
/// Utility class for managing GitHub repository secrets
/// </summary>
public sealed class GitHubRepositoriesSecretsUtil : IGitHubRepositoriesSecretsUtil
{
    private readonly ILogger<GitHubRepositoriesSecretsUtil> _logger;
    private readonly IGitHubOpenApiClientUtil _gitHubClientUtil;

    public GitHubRepositoriesSecretsUtil(ILogger<GitHubRepositoriesSecretsUtil> logger, IGitHubOpenApiClientUtil gitHubClientUtil)
    {
        _logger = logger;
        _gitHubClientUtil = gitHubClientUtil;
    }

    /// <summary>
    /// Gets all secrets for a repository
    /// </summary>
    public async ValueTask<List<ActionsSecret>> Get(string owner, string repo, CancellationToken cancellationToken = default)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            SecretsGetResponse? response = await client.Repos[owner][repo].Actions.Secrets.GetAsSecretsGetResponseAsync(cancellationToken: cancellationToken).NoSync();
            return response?.Secrets ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting secrets for repository {Owner}/{Repo}", owner, repo);
            throw;
        }
    }

    public async ValueTask<List<ActionsSecret>> GetOrganization(string owner, string repo, CancellationToken cancellationToken = default)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            OrganizationSecretsGetResponse? response = await client.Repos[owner][repo].Actions.OrganizationSecrets.GetAsOrganizationSecretsGetResponseAsync(cancellationToken: cancellationToken).NoSync();
            return response?.Secrets ?? new List<ActionsSecret>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting organization secrets for repository {Owner}/{Repo}", owner, repo);
            throw;
        }
    }

    public async ValueTask<ActionsSecret> Get(string owner, string repo, string name, CancellationToken cancellationToken = default)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            return await client.Repos[owner][repo].Actions.Secrets[name].GetAsync(cancellationToken: cancellationToken).NoSync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting secret {Name} for repository {Owner}/{Repo}", name, owner, repo);
            throw;
        }
    }

    public async ValueTask<(string KeyId, string Key)> GetPublicKey(string owner, string repo, CancellationToken cancellationToken = default)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            ActionsPublicKey? response = await client.Repos[owner][repo].Actions.Secrets.PublicKey.GetAsync(cancellationToken: cancellationToken).NoSync();
            return (response.KeyId, response.Key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting public key for repository {Owner}/{Repo}", owner, repo);
            throw;
        }
    }

    public async ValueTask CreateOrUpdate(string owner, string repo, string name, string value, CancellationToken cancellationToken = default)
    {
        try
        {
            (string keyId, string publicKey) = await GetPublicKey(owner, repo, cancellationToken).NoSync();

            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            byte[] encryptedBytes = SealedPublicKeyBox.Create(System.Text.Encoding.UTF8.GetBytes(value), publicKeyBytes);

            string encryptedValue = Convert.ToBase64String(encryptedBytes);

            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();

            var requestBody = new WithSecret_namePutRequestBody
            {
                EncryptedValue = encryptedValue,
                KeyId = keyId
            };

            await client.Repos[owner][repo].Actions.Secrets[name].PutAsync(requestBody, cancellationToken: cancellationToken).NoSync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating/updating secret {Name} for repository {Owner}/{Repo}", name, owner, repo);
            throw;
        }
    }

    public async ValueTask Delete(string owner, string repo, string name, CancellationToken cancellationToken = default)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            await client.Repos[owner][repo].Actions.Secrets[name].DeleteAsync(cancellationToken: cancellationToken).NoSync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting secret {Name} for repository {Owner}/{Repo}", name, owner, repo);
            throw;
        }
    }
}