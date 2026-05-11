using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    internal static class ValidatorProjectNameHttpApi
    {
        internal static IServiceCollection AddProjectNameValidators(this IServiceCollection services)
        {

            services
                .AddValidatorsFromAssemblyContaining(typeof(ProjectNameHttpApi));

            return services;
        }
    }
}