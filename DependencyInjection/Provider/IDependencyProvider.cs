namespace DependencyInjection.Provider
{
    public interface IDependencyProvider
    {
        TDependency Resolve<TDependency>(string name = null) where TDependency : class;
    }
}