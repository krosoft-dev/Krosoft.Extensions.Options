using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Options.Services;

internal sealed class ScopedValidateOptions<TSettings, TValidateOptions>(IServiceScopeFactory scopeFactory) : IValidateOptions<TSettings>
    where TSettings : class
    where TValidateOptions : class, IValidateOptions<TSettings>
{
    public ValidateOptionsResult Validate(string? name, TSettings options)
    {
        using var scope = scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TValidateOptions>().Validate(name, options);
    }
}
