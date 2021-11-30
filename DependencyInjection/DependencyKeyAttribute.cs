using System;

namespace DependencyInjection
{
    public class DependencyKeyAttribute: Attribute
    {
        public string Name { get; }

        public DependencyKeyAttribute(string name)
        {
            Name = name;
        }
    }
}