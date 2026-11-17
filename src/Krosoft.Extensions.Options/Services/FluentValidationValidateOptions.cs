using FluentValidation;

namespace Krosoft.Extensions.Options.Services;

internal sealed class FluentValidationValidateOptions<TSettings>(IValidator<TSettings> validator)
    : SettingsValidateOptions<TSettings>(validator)
    where TSettings : class;
