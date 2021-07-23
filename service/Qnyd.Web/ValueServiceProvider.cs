using System;
using System.Collections.Generic;

namespace Structing.Core
{
    internal class ValueServiceProvider : IServiceProvider
    {
        public ValueServiceProvider()
            :this(null)
        {

        }
        public ValueServiceProvider(IServiceProvider innerProvider)
        {
            InnerProvider = innerProvider;
            Factories = new Dictionary<Type, Func<IServiceProvider, object>>();
        }

        public IDictionary<Type,Func<IServiceProvider,object>> Factories { get; }

        public IServiceProvider InnerProvider { get; }

        public object GetService(Type serviceType)
        {
            if (Factories.TryGetValue(serviceType,out var factory))
            {
                return factory(this);
            }
            return InnerProvider?.GetService(serviceType);
        }
    }
}
