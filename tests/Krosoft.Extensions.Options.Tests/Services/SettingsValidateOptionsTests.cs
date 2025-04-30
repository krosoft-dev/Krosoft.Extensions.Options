//using FluentValidation;
//using Krosoft.Extensions.Options.Extensions;
//using Krosoft.Extensions.Options.Services;
//using Krosoft.Extensions.Testing;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace Krosoft.Extensions.Options.Tests.Services;

//[TestClass]
//public class SettingsValidateOptionsTests : BaseTest
//{

//    //[TestMethod]
//    //public void Handle_Empty()
//    //{
//    //    Check.ThatCode(() =>
//    //    {
//    //        using var serviceProvider = CreateServiceCollection();
//    //        return this.SendQueryAsync(serviceProvider, new CompteRenduQuery());
//    //    })
//    //         .Throws<KrosoftFunctionalException>()
//    //         .WithMessage("'Numero Depot' ne doit pas être vide.")
//    //         .And.WhichMember(x => x.Errors)
//    //         .ContainsExactly("'Numero Depot' ne doit pas être vide.",
//    //                          "'Numero Depot' ne doit pas avoir la valeur null.");
//    //}

//    //[TestMethod]
//    //public async Task Handle_Ko_Depot_Introuvable()
//    //{
//    //    var queryByName = new CompteRenduQuery
//    //    {
//    //        NumeroDepot = "Test"
//    //    };

//    //    await using var serviceProvider = CreateServiceCollection();

//    //    SetupMock("consulterCRDetaille_ko_depot-introuvable");

//    //    var helloDto = await this.SendQueryAsync(serviceProvider, queryByName);
//    //    Check.That(helloDto).IsNotNull();
//    //    Check.That(helloDto.Code).IsEqualTo("20001");
//    //    Check.That(helloDto.Message).IsEqualTo("POQ_MSG_01.003 - Flux introuvable.");
//    //    Check.That(helloDto.Erreurs).IsEmpty();
//    //    _mockLogger.VerifyWasCalled(LogLevel.Information, "Recuperation du compte rendu pour le document 'Test'...", Times.Once());
//    //}

//    //[TestMethod]
//    //public async Task Handle_Ko_Erreurs_Depot()
//    //{
//    //    var queryByName = new CompteRenduQuery
//    //    {
//    //        NumeroDepot = "Test"
//    //    };

//    //    await using var serviceProvider = CreateServiceCollection();

//    //    SetupMock("consulterCRDetaille_ko_erreurs-depot");

//    //    var compteRenduDto = await this.SendQueryAsync(serviceProvider, queryByName);
//    //    Check.That(compteRenduDto).IsNotNull();
//    //    Check.That(compteRenduDto.Code).IsEqualTo("0");
//    //    Check.That(compteRenduDto.Message).IsEqualTo("TRA_MSG_00.000");
//    //    Check.That(compteRenduDto.Erreurs).HasSize(1);

//    //    var erreur = compteRenduDto.Erreurs.First();
//    //    Check.That(erreur.Code).IsEqualTo(null);
//    //    Check.That(erreur.Libelle).IsEqualTo("L'identifiant fournisseur de la demande de paiement (balise : AccountingSupplierParty.Party.PartyIdentification.ID.value) n'est pas reference dans notre systeme.|La structure (balise AccountingSupplierParty.Party.PartyIdentification.ID.value) n'utilise pa");
//    //    _mockLogger.VerifyWasCalled(LogLevel.Information, "Recuperation du compte rendu pour le document 'Test'...", Times.Once());
//    //}

//    //[TestMethod]
//    //public async Task Handle_Ko_Erreurs_Technique()
//    //{
//    //    var queryByName = new CompteRenduQuery
//    //    {
//    //        NumeroDepot = "Test"
//    //    };

//    //    await using var serviceProvider = CreateServiceCollection();

//    //    SetupMock("consulterCRDetaille_ko_erreurs-technique");

//    //    var compteRenduDto = await this.SendQueryAsync(serviceProvider, queryByName);
//    //    Check.That(compteRenduDto).IsNotNull();
//    //    Check.That(compteRenduDto.Code).IsEqualTo("0");
//    //    Check.That(compteRenduDto.Message).IsEqualTo("TRA_MSG_00.000");
//    //    Check.That(compteRenduDto.Erreurs).HasSize(53);

//    //    var erreur = compteRenduDto.Erreurs.First();
//    //    Check.That(erreur.Code).IsEqualTo("ERR_XSD_001");
//    //    Check.That(erreur.Libelle).IsEqualTo("IRRECEVABILITE: cvc-datatype-valid.1.2.1: '-1 324,60' is not a valid value for 'decimal'. - ligne: 150 - position: 60");
//    //    _mockLogger.VerifyWasCalled(LogLevel.Information, "Recuperation du compte rendu pour le document 'Test'...", Times.Once());
//    //}

//    //[TestMethod]
//    //public async Task Handle_Ok()
//    //{
//    //    var queryByName = new CompteRenduQuery
//    //    {
//    //        NumeroDepot = "Test"
//    //    };

//    //    await using var serviceProvider = CreateServiceCollection();

//    //    SetupMock("consulterCRDetaille_ok");

//    //    var helloDto = await this.SendQueryAsync(serviceProvider, queryByName);
//    //    Check.That(helloDto).IsNotNull();
//    //    Check.That(helloDto.Code).IsEqualTo("0");
//    //    Check.That(helloDto.Message).IsEqualTo("TRA_MSG_00.000");
//    //    Check.That(helloDto.Erreurs).IsEmpty();
//    //    _mockLogger.VerifyWasCalled(LogLevel.Information, "Recuperation du compte rendu pour le document 'Test'...", Times.Once());
//    //}

//    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
//    {
//        //var assemblyCore = typeof(ServiceCollectionExtensions).Assembly;
//        //var assemblyApi = typeof(WebApplicationExtensions).Assembly;

//        services. AddOptionsValidator<AppSettings, AppSettingsValidateOptions>(configuration)
//            ;

//        //_mockLogger = new Mock<ILogger<CompteRenduQueryHandler>>();
//        //services.SwapTransient(_ => _mockLogger.Object);

//        //var mockAuthService = new Mock<IAuthService>();
//        //mockAuthService.Setup(x => x.GetAuthAsync(It.IsAny<ChorusSettings>(), It.IsAny<CancellationToken>()))
//        //               .ReturnsAsync(() => new AuthBusiness("Hello", "World"));
//        //services.SwapTransient(_ => mockAuthService.Object);

//        //_mockApiHttpService = new Mock<IApiHttpService>();

//        //services.SwapTransient(_ => _mockApiHttpService.Object);

//        base.AddServices(services, configuration);
//    }

//    //private void SetupMock(string fileName)
//    //{
//    //    _mockApiHttpService.Setup(x => x.GetDepotCrDetailleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
//    //                                                             It.IsAny<CancellationToken>()))
//    //                       .ReturnsAsync(() =>
//    //                       {
//    //                           var json = File.ReadAllText($"./Files/{fileName}.json");

//    //                           var crDetailleHttp = JsonConvert.DeserializeObject<CrDetailleHttp>(json);

//    //                           return crDetailleHttp;
//    //                       });
//    //}

//    [TestMethod()]
//    public void SettingsValidateOptionsTest()
//    {
//        Assert.Fail();
//    }

//    [TestMethod()]
//    public void ValidateTest()
//    {
//        Assert.Fail();
//    }
//}



//internal record AppSettings
//{
//    public string BaseUrl { get; set; } = null!; 
//}

//internal class AppSettingsValidateOptions(IValidator<AppSettings> validator) : SettingsValidateOptions<AppSettings>(validator);

using FluentValidation;
using Krosoft.Extensions.Options.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Options.Tests.Services;

[TestClass]
public class SettingsValidateOptionsIntegrationTests
{
    private class MySettings
    {
        public string? Name { get; set; }
    }

    private class MySettingsValidator : AbstractValidator<MySettings>
    {
        public MySettingsValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Le nom est obligatoire");
        }
    }

    [TestMethod]
    public void IOptions_Should_Return_Validation_Error_When_Settings_Invalid()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddSingleton<IValidator<MySettings>, MySettingsValidator>();
        services.AddSingleton<IValidateOptions<MySettings>, MySettingsValidation>();
        services.Configure<MySettings>(options => options.Name = "");

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<MySettings>>();

        // Act
        var act = () => options.Value; // va lancer une exception si invalide

        // Assert
        Check.ThatCode(act)
             .Throws<OptionsValidationException>()
             .WithMessage("Le nom est obligatoire");
    }

    private class MySettingsValidation : SettingsValidateOptions<MySettings>
    {
        public MySettingsValidation(IValidator<MySettings> validator) : base(validator) { }
    }
}