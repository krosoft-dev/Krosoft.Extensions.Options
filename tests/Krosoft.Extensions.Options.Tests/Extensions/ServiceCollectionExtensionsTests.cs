using FluentValidation;
using JetBrains.Annotations;
using Krosoft.Extensions.Options.Extensions;
using Krosoft.Extensions.Options.Services;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceCollectionExtensions = Krosoft.Extensions.Options.Extensions.ServiceCollectionExtensions;

namespace Krosoft.Extensions.Options.Tests.Extensions;

[TestClass]
[TestSubject(typeof(ServiceCollectionExtensions))]
public class ServiceCollectionExtensionsTests : BaseTest
{
    //TestInitialize
    private IOptions<SampleSettings> _options = null!;

    [TestMethod]
    public void SampleSettings_Ok()
    {
        Check.That(_options.Value.BaseUrl).IsEqualTo("https://api.com");
        Check.That(_options.Value.Username).IsEqualTo("hello");
        Check.That(_options.Value.Password).IsEqualTo("world");
    }

    [TestMethod]
    public void AddOptionsValidator_SansClasseDeValidation_RetourneLesOptions()
    {
        var options = BuildOptions(new Dictionary<string, string?>
        {
            [$"{nameof(SampleSettings)}:{nameof(SampleSettings.BaseUrl)}"] = "https://api.com",
            [$"{nameof(SampleSettings)}:{nameof(SampleSettings.Username)}"] = "hello",
            [$"{nameof(SampleSettings)}:{nameof(SampleSettings.Password)}"] = "world"
        });

        Check.That(options.Value.Username).IsEqualTo("hello");
    }

    [TestMethod]
    public void AddOptionsValidator_SansClasseDeValidation_LeveUneErreurSiInvalide()
    {
        var options = BuildOptions(new Dictionary<string, string?>
        {
            [$"{nameof(SampleSettings)}:{nameof(SampleSettings.BaseUrl)}"] = "https://api.com"
        });

        Check.ThatCode(() => options.Value)
             .Throws<OptionsValidationException>();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _options = serviceProvider.GetRequiredService<IOptions<SampleSettings>>();
    }

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblies([typeof(ServiceCollectionExtensionsTests).Assembly], includeInternalTypes: true);
        services.AddOptionsValidator<SampleSettings, SampleSettingsValidateOptions>(configuration);
    }

    private static IOptions<SampleSettings> BuildOptions(Dictionary<string, string?> values)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();
        var services = new ServiceCollection();
        services.AddOptionsValidator<SampleSettings>(configuration)
                .AddScoped<IValidator<SampleSettings>, SampleSettingsValidator>();

        var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });
        return serviceProvider.GetRequiredService<IOptions<SampleSettings>>();
    }

    public record SampleSettings
    {
        public string BaseUrl { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    internal class SampleSettingsValidateOptions(IValidator<SampleSettings> validator) : SettingsValidateOptions<SampleSettings>(validator);

    internal class SampleSettingsValidator : AbstractValidator<SampleSettings>
    {
        public SampleSettingsValidator()
        {
            RuleFor(v => v.BaseUrl)
                .NotEmpty();
            RuleFor(v => v.Username)
                .NotEmpty();
            RuleFor(v => v.Password)
                .NotEmpty();
        }
    }
}