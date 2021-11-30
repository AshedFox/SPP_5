using System.Collections.Generic;

namespace DependencyInjection.Tests.TestTypes.Interfaces
{
    public interface ITestGenericCollection<T>
    {
        public List<T> Collection { get; set; }

        void Add(T item);
    }
}