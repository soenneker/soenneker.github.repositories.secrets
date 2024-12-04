using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GitHub.Client.Registrars;
using Soenneker.GitHub.Repositories.Secrets.Abstract;

namespace Soenneker.GitHub.Repositories.Secrets.Registrars;

/// <summary>
/// A utility library for GitHub Repository Secret related operations
/// </summary>
public static class GitHubRepositoriesSecretsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGitHubRepositoriesSecretsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static void AddGitHubRepositoriesSecretsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddGitHubClientUtilAsSingleton();
        services.TryAddSingleton<IGitHubRepositoriesSecretsUtil, GitHubRepositoriesSecretsUtil>();
    }

    /// <summary>
    /// Adds <see cref="IGitHubRepositoriesSecretsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddGitHubRepositoriesSecretsUtilAsScoped(this IServiceCollection services)
    {
        services.AddGitHubClientUtilAsSingleton();
        services.TryAddScoped<IGitHubRepositoriesSecretsUtil, GitHubRepositoriesSecretsUtil>();
    }
}
