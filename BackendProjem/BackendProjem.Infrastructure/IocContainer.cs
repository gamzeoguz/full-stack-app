using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BackendProjem.Infrastructure
{
    public class IocContainer
    {
        private static IServiceCollection _services;
        private static ServiceProvider _provider;

        public static void Init(IServiceCollection services)
        {
            _services = services;

            _provider = _services.BuildServiceProvider();
        }

        public static TInstance Resolve<TInstance>()
        {
            return _provider.GetService<TInstance>();
        }

        public static object Resolve(Type type)
        {
            return _provider.GetService(type);
        }
    }

    public static class IocContainerExtensions
    {
        public static void AddIocContainer(this IServiceCollection services)
        {
            IocContainer.Init(services);
        }
    }
}
