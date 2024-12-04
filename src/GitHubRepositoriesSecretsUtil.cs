using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.GitHub.Client.Abstract;
using Soenneker.GitHub.Repositories.Secrets.Abstract;

namespace Soenneker.GitHub.Repositories.Secrets;

///<inheritdoc cref="IGitHubRepositoriesSecretsUtil"/>
public class GitHubRepositoriesSecretsUtil : IGitHubRepositoriesSecretsUtil
{
    private readonly ILogger<GitHubRepositoriesSecretsUtil> _logger;
    private readonly IGitHubClientUtil _gitHubClientUtil;

    public GitHubRepositoriesSecretsUtil(ILogger<GitHubRepositoriesSecretsUtil> logger, IGitHubClientUtil gitHubClientUtil)
    {
        _logger = logger;
        _gitHubClientUtil = gitHubClientUtil;
    }

    public async ValueTask SetSecret(string projectName, string gitHubUsername, string secretName, string secretValue, bool log = true, CancellationToken cancellationToken = default)
    {
        secretValue.ThrowIfNullOrEmpty(nameof(secretValue));
        secretValue.ThrowIfNullOrEmpty(nameof(secretName));

        if (log)
            _logger.LogInformation("Setting Secret on repo ({repo}): {secretName} // {secretValue} ...", projectName, secretName, secretValue.Mask());

        byte[] secretValueBytes = secretValue.ToBytes();

        GitHubClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();

        SecretsPublicKey? secretsPublicKey = await client.Repository.Actions.Secrets.GetPublicKey(gitHubUsername, projectName).NoSync();

        byte[] publicKeyBytes = Convert.FromBase64String(secretsPublicKey!.Key);

        byte[] sealedPublicKeyBox = Sodium.SealedPublicKeyBox.Create(secretValueBytes, publicKeyBytes);

        string encryptedValue = Convert.ToBase64String(sealedPublicKeyBox);

        await client.Repository.Actions.Secrets.CreateOrUpdate(gitHubUsername, projectName, secretName, new UpsertRepositorySecret
        {
            EncryptedValue = encryptedValue,
            KeyId = secretsPublicKey.KeyId
        }).NoSync();
    }
}