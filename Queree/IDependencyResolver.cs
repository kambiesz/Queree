using System;
using System.Collections.Generic;
using System.Text;

namespace Queree
{
    public interface IDependencyResolver
    {
        bool CanResolve<T>();
        IEnumerable<T> ResolveAll<T>();
        T Resolve<T>();
    }
}
