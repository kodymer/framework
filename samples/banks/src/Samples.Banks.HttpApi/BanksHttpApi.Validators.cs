using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.AccountManagement;
using Samples.Banks.Endpoints.AccountManagement;

namespace Samples.Banks
{
    internal static class ValidatorBanksHttpApi
    {
        internal static IServiceCollection AddBanksValidators(this IServiceCollection services)
        {

            //services
            //    .AddFluentValidationAutoValidation(config =>
            //    {
            //        config.DisableDataAnnotationsValidation = true;
            //    });
                //.AddValidatorsFromAssembly(typeof(BanksHttpApi).Assembly);

            services.AddScoped<IValidator<CreateBankAccountCommand>, CreateBankAccountCommandValidator>();

            return services;
        }
    }
}