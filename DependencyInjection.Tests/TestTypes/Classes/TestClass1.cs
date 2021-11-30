using DependencyInjection.Tests.TestTypes.Interfaces;

namespace DependencyInjection.Tests.TestTypes.Classes
{
    public class TestClass1: ITestInterface
    {
        private TestClass1()
        {
            Value = 0;
        }

        public int Value { get; set; }
    }
}