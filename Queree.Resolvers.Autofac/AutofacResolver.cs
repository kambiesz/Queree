using Autofac;
using System;
using System.Collections.Generic;

namespace Queree.Resolvers.Autofac
{
    public class AutofacResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacResolver(IContainer container)
        {
            _container = container;
        }

        public bool CanResolve<T>()
        {
            return _container.IsRegistered<T>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.Resolve<IEnumerable<T>>();
        }
    }
}
