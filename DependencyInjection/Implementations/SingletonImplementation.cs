using System;
using DependencyInjection.Config;
using DependencyInjection.TypeGenerator;

namespace DependencyInjection.Implementations
{
    public class SingletonImplementation : Implementation
    {
        public SingletonImplementation(string name, Type type) : base(name, type, Lifetime.Singleton)
        { }

        private static readonly object SyncRoot = new();
        private static object _instance = new();

        public override object GetInstance(ITypeGenerator generator)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    _instance ??= generator.Generate(this);
                }
            }
            return _instance;
        }
    }
}