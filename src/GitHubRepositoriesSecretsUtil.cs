using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Soenneker.Extensions.Arrays.Bytes;
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

    public async ValueTask Upsert(string projectName, string gitHubUsername, string secretName, string secretValue, bool log = true, CancellationToken cancellationToken = default)
    {
        secretValue.ThrowIfNullOrEmpty(nameof(secretValue));
        secretName.ThrowIfNullOrEmpty(nameof(secretName));

        if (log)
            _logger.LogInformation("Setting Secret on repo ({repo}): {secretName} // {secretValue} ...", projectName, secretName, secretValue.Mask());

        byte[] secretValueBytes = secretValue.ToBytes();

        GitHubClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();

        SecretsPublicKey? secretsPublicKey = await client.Repository.Actions.Secrets.GetPublicKey(gitHubUsername, projectName).NoSync();

        byte[] publicKeyBytes = secretsPublicKey!.Key.ToBytesFromBase64();

        byte[] sealedPublicKeyBox = Sodium.SealedPublicKeyBox.Create(secretValueBytes, publicKeyBytes);

        string encryptedValue = sealedPublicKeyBox.ToBase64String();

        await client.Repository.Actions.Secrets.CreateOrUpdate(gitHubUsername, projectName, secretName, new UpsertRepositorySecret
        {
            EncryptedValue = encryptedValue,
            KeyId = secretsPublicKey.KeyId
        }).NoSync();
    }
}