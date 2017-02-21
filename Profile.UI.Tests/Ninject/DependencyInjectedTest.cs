using Ninject;

namespace Profile.UI.Tests.Ninject
{
    public abstract class DependencyInjectedTest
    {
        protected DependencyInjectedTest()
        {
            Kernel = new StandardKernel(new NinjectTestingModule());
        }

        protected IKernel Kernel { get; }

        /// <summary>
        /// Creates a new custom scope for dependency injection
        /// </summary>
        protected void NewScope()
        {
            ProcessingScope.Current = new ScopeObject();
        }
    }
}
