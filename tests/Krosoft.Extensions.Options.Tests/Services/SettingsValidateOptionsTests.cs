using FluentValidation;
using Krosoft.Extensions.Options.Extensions;
using Krosoft.Extensions.Options.Services;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Options.Tests.Services;

[TestClass]
public class SettingsValidateOptionsTests : BaseTest
{
    [TestMethod]
    public void IOptions_Should_Return_Ok()
    {
        var serviceProvider = CreateServiceCollection(d =>
        {
            d.Configure<MySettings>(options =>
            {
                options.Token = "MyToken";
                options.BaseUrl = "Trst";
            });
        });
        var options = serviceProvider.GetRequiredService<IOptions<MySettings>>();

        Check.That(options.Value).IsNotNull();
    }

    [TestMethod]
    public void IOptions_With_Child_Should_Return_Ok()
    {
        var serviceProvider = CreateServiceCollection(d =>
        {
            d.Configure<MyFullSettings>(options =>
            {
                options.Token = "MyToken";
                options.BaseUrl = "Trst";
                options.Settings = new MyChildSettings
                {
                    Delay = TimeSpan.FromHours(1)
                };
            });
        });
        var options = serviceProvider.GetRequiredService<IOptions<MyFullSettings>>();

        Check.That(options.Value).IsNotNull();
    }

    [TestMethod]
    public void IOptions_With_Child_Should_Return_Validation_Error_When_Settings_Invalid()
    {
        var serviceProvider = CreateServiceCollection(d =>
        {
            d.Configure<MyFullSettings>(options =>
            {
                options.Token = "MyToken";
                options.BaseUrl = "Trst";
            });
        });
        var options = serviceProvider.GetRequiredService<IOptions<MyFullSettings>>();
        Check.ThatCode(() => options.Value)
             .Throws<OptionsValidationException>()
             .WithMessage("'Settings' ne doit pas être vide.");
    }

    [TestMethod]
    public void IOptions_Should_Return_Validation_Error_When_Settings_Empty()
    {
        var serviceProvider = CreateServiceCollection();
        var options = serviceProvider.GetRequiredService<IOptions<MySettings>>();

        Check.ThatCode(() => options.Value)
             .Throws<OptionsValidationException>()
             .WithMessage("Le token est obligatoire.; 'Base Url' ne doit pas être vide.");
    }

    [TestMethod]
    public void IOptions_Should_Return_Validation_Error_When_Settings_Invalid()
    {
        var serviceProvider = CreateServiceCollection(d => { d.Configure<MySettings>(options => options.Token = "MyToken"); });
        var options = serviceProvider.GetRequiredService<IOptions<MySettings>>();

        Check.ThatCode(() => options.Value)
             .Throws<OptionsValidationException>()
             .WithMessage("'Base Url' ne doit pas être vide.");
    }

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsValidator<MySettings, MySettingsValidateOptions>(configuration)
                .AddOptionsValidator<MyFullSettings, MyFullSettingsValidateOptions>(configuration)
                .AddSingleton<IValidator<MySettings>, MySettingsValidator>()
                .AddSingleton<IValidator<MyFullSettings>, MyFullSettingsValidator>()
                .AddSingleton<IValidator<MyChildSettings>, MyChildSettingsValidator>();

        base.AddServices(services, configuration);
    }

    private class MyFullSettings
    {
        public string? Token { get; set; }
        public string BaseUrl { get; set; } = null!;
        public MyChildSettings Settings { get; set; } = null!;
    }

    private class MySettings
    {
        public string? Token { get; set; }
        public string BaseUrl { get; set; } = null!;
    }

    private class MyChildSettings
    {
        public TimeSpan? Delay { get; set; }
    }

    private class MyFullSettingsValidator : AbstractValidator<MyFullSettings>
    {
        public MyFullSettingsValidator(IValidator<MyChildSettings> validator)
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Le token est obligatoire.");
            RuleFor(x => x.BaseUrl).NotEmpty();
            RuleFor(x => x.Settings).NotEmpty().SetValidator(validator);
        }
    }

    private class MySettingsValidator : AbstractValidator<MySettings>
    {
        public MySettingsValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Le token est obligatoire.");
            RuleFor(x => x.BaseUrl).NotEmpty();
        }
    }

    private class MyChildSettingsValidator : AbstractValidator<MyChildSettings>
    {
        public MyChildSettingsValidator()
        {
            RuleFor(x => x.Delay).NotEmpty();
        }
    }

    private class MySettingsValidateOptions(IValidator<MySettings> validator) : SettingsValidateOptions<MySettings>(validator);

    private class MyFullSettingsValidateOptions(IValidator<MyFullSettings> validator) : SettingsValidateOptions<MyFullSettings>(validator);
}