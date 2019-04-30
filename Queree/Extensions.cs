using Microsoft.Extensions.DependencyInjection;
using Queree.Commands;
using Queree.Events;
using Queree.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queree
{
    public static class Extensions
    {
        public static IServiceCollection AddQueree(this IServiceCollection services, IDependencyResolver resolver, IQueryCache queryCache)
        {
            services.AddSingleton(resolver);
            services.AddSingleton(queryCache);
            services.AddSingleton<ICommandProcessor, CommandProcessor>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<IQueryRunner, QueryRunner>();

            return services;
        }
    }
}
