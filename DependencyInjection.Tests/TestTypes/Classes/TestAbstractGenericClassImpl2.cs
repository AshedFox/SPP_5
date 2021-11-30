using DependencyInjection.Tests.TestTypes.Abstract;
using DependencyInjection.Tests.TestTypes.Interfaces;

namespace DependencyInjection.Tests.TestTypes.Classes
{
    public class TestAbstractGenericClassImpl2<T> : TestAbstractGenericClass<T>
    {
        public TestAbstractGenericClassImpl2([DependencyKey("name")] ITestInterface value) : base(value)
        { }
        
        public override void DoSomething1(T item)
        { }
    }
}