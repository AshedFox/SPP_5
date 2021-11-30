using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DependencyInjection.Config;
using DependencyInjection.Implementations;

namespace DependencyInjection.TypeGenerator
{
    public class TypeGenerator: ITypeGenerator
    {
        private readonly IDependenciesConfig _config;

        public TypeGenerator(IDependenciesConfig config)
        {
            _config = config;
        }

        public object Generate(Implementation implementation, Type genericParamType = null)
        {
            if (implementation.Type.IsGenericTypeDefinition && genericParamType is null)
            {
                throw new ArgumentException("If implementation has open generic type generic param type can't be null");
            }
            
            var type = implementation.Type.IsGenericTypeDefinition
                ? implementation.Type.MakeGenericType(genericParamType)
                : implementation.Type;
            
            var constructors =
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            var constructor = constructors.OrderByDescending(constructor => 
                constructor.GetParameters().Count(param => _config.Implementations.ContainsKey(param.ParameterType)))
                .FirstOrDefault();
            
            if (constructor is null)
            {
                throw new ArgumentException($"{type} doesn't have accessible constructor");
            }
            
            var arguments = GenerateConstructorParams(constructor);


            return constructor.Invoke(arguments.ToArray());
            
        }

        public IEnumerable<object> GenerateCollection(ICollection<Implementation> implementations, Type dependencyType, 
            Type genericParamType = null)
        {
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(dependencyType));
            foreach (var implementation in implementations)
            {
                if (list != null)
                {
                    list.Add(Generate(implementation, genericParamType));
                }
            }

            return (IEnumerable<object>)list;
        }
        
        private IEnumerable<object> GenerateConstructorParams(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            return parameters.Select(GenerateConstructorParam);
        }


        private object GenerateConstructorParam(ParameterInfo param)
        {
            _config.Implementations.TryGetValue(param.ParameterType, out var implementations);
            
            if (implementations is null)
            {
                return default;
            }

            var attribute = param.GetCustomAttribute(typeof(DependencyKeyAttribute));
            var name = (attribute as DependencyKeyAttribute)?.Name;
            var implementation = name is null ? implementations.FirstOrDefault() :
                implementations.FirstOrDefault(implementation => implementation.Name == name);

            if (implementation is null)
            {
                throw new ArgumentException(
                    $"There is no implementation for {param.ParameterType} with necessary name");
            }

            return implementation.GetInstance(this);
        }
    }
}