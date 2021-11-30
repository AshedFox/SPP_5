using DependencyInjection.Tests.TestTypes.Interfaces;

namespace DependencyInjection.Tests.TestTypes.Abstract
{
    public abstract class TestAbstractGenericClass<T>
    {
        protected TestAbstractGenericClass(ITestInterface value)
        {
            Value = value;
        }
        
        public ITestInterface Value { get; set; }
        
        public abstract void DoSomething1(T item);
    }
}