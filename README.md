[![](https://img.shields.io/nuget/v/soenneker.github.repositories.secrets.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.repositories.secrets/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.repositories.secrets/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.github.repositories.secrets/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.github.repositories.secrets.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.repositories.secrets/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.repositories.secrets/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.github.repositories.secrets/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.GitHub.Repositories.Secrets

List GitHub Actions secret metadata and securely create, replace, or delete repository secrets.

## Installation

```bash
dotnet add package Soenneker.GitHub.Repositories.Secrets
```

## Configuration

```json
{
  "GH": {
    "Token": "github-token"
  }
}
```

The token needs GitHub Actions secrets read or write permission for the target repository.

## Registration

```csharp
services.AddGitHubRepositoriesSecretsUtilAsSingleton();
```

Use `AddGitHubRepositoriesSecretsUtilAsScoped()` for a scoped consumer.

## Create or replace a secret

```csharp
await secrets.CreateOrUpdate(
    owner: "soenneker",
    repo: "example-repository",
    name: "NUGET__TOKEN",
    value: nugetToken,
    cancellationToken);
```

`CreateOrUpdate` retrieves the repository's current public key, encrypts the UTF-8 value with a libsodium sealed box, and sends only the encrypted value to GitHub.

## List secret metadata

```csharp
List<ActionsSecret> repositorySecrets = await secrets.GetAll(
    "soenneker",
    "example-repository",
    cancellationToken);

List<ActionsSecret> organizationSecrets = await secrets.GetOrganization(
    "soenneker",
    "example-repository",
    cancellationToken);
```

Both list methods follow pagination. `GetOrganization` returns organization-level Actions secrets made available to the specified repository.

GitHub never returns secret values after storage. These methods return names and metadata only. The four-argument `GetAll(owner, repo, name, ...)` overload retrieves metadata for one named repository secret.

`Delete` permanently removes the named repository secret. It does not affect an organization secret with the same name.
