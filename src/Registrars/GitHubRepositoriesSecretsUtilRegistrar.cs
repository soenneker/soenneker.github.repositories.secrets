using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GitHub.ClientUtil.Registrars;
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
    public static IServiceCollection AddGitHubRepositoriesSecretsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddGitHubOpenApiClientUtilAsSingleton()
                .TryAddSingleton<IGitHubRepositoriesSecretsUtil, GitHubRepositoriesSecretsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IGitHubRepositoriesSecretsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddGitHubRepositoriesSecretsUtilAsScoped(this IServiceCollection services)
    {
        services.AddGitHubOpenApiClientUtilAsSingleton()
                .TryAddScoped<IGitHubRepositoriesSecretsUtil, GitHubRepositoriesSecretsUtil>();

        return services;
    }
}