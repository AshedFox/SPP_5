using DependencyInjection.Tests.TestTypes.Interfaces;

namespace DependencyInjection.Tests.TestTypes.Classes
{
    public class TestClass2 : ITestInterface
    {
        public int Value { get; set; }
    }
}