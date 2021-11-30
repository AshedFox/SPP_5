using System.Collections.Generic;
using DependencyInjection.Tests.TestTypes.Interfaces;

namespace DependencyInjection.Tests.TestTypes.Classes
{
    public class TestGenericCollection1<T> : ITestGenericCollection<T>
    {
        public List<T> Collection { get; set; }
        public void Add(T item)
        {
            Collection.Add(item);
        }
    }
}