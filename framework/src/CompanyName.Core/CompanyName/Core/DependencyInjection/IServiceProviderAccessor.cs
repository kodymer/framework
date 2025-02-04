namespace CompanyName.Core.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; }
    }
}