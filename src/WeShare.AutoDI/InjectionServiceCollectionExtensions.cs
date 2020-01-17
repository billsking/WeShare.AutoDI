using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 继承系统默认接口IAutoDIable自动注入。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AutoDI(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AutoDI(typeof(IAutoDIable));
        }
        /// <summary>
        /// 继承自定义泛型接口自动注入。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static IServiceCollection AutoDI(this IServiceCollection services, Type baseType, params Assembly[] assemblies)
        {
            var allAssemblies = assemblies.ToList();
            if (assemblies.Length == 0)
            {
                allAssemblies = TypeExtensions.GetCurrentPathAssembly();
            }
            foreach (var assembly in allAssemblies)
            {
                var classz = assembly.GetTypes()
                    .Where(type => type.IsClass
                                   && type.BaseType != null
                                   && type.HasImplementedRawGeneric(baseType));
                foreach (var type in classz)
                {
                    var interfaces = type.GetInterfaces();
                    #region 获取生命周期
                    ServiceLifetime lifetime = ServiceLifetime.Scoped;
                    var autoDIableType = typeof(IAutoDIable);
                    var lifetimeInterface = interfaces.FirstOrDefault(x =>
                        x.FullName != baseType.FullName
                        && x.FullName != autoDIableType.FullName
                        && x.Name.EndsWith("AutoDIable")
                    );
                    if (lifetimeInterface != null)
                    {
                        switch (lifetimeInterface.Name)
                        {
                            case nameof(IScopedAutoDIable):
                                lifetime = ServiceLifetime.Scoped;
                                break;
                            case nameof(ITransientAutoDIable):
                                lifetime = ServiceLifetime.Transient;
                                break;
                            case nameof(ISingletonAutoDIable):
                                lifetime = ServiceLifetime.Singleton;
                                break;
                        }
                    }
                    #endregion
                    var interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                    if (interfaceType == null)
                    {
                        interfaceType = type;
                    }
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interfaceType, type, lifetime);
                    if (!services.Contains(serviceDescriptor))
                    {
                        services.TryAdd(serviceDescriptor);
                    }
                }
            }
            return services;
        }
        /// <summary>
        /// 继承自定义接口自动注入。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AutoDI<T>(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AutoDI(typeof(T));
        }
    }
}