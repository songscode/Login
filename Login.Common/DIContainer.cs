using Autofac;
using Autofac.Core;
namespace Login.Common
{
    ///<summary>
    ///依赖注入容器
    ///</summary>
    ///<remarks>
    ///对Autofac进行封装
    ///</remarks>
    public class DIContainer
    {
        private static IContainer _container;

        ///<summary>注册DIContainer
        ///</summary>
        ///<param name="container">Autofac.IContainer</param>
        public static void RegisterContainer(IContainer container)
        {
            _container = container;
        }

        ///<summary>按类型获取组件
        ///</summary>
        ///<typeparam name="TService">组件类型</typeparam>
        ///<returns>返回获取的组件</returns>
        public static TService Resolve<TService>()
        {
            using (ILifetimeScope scope = _container.BeginLifetimeScope())
            {
                return ResolutionExtensions.Resolve<TService>(scope);
            }
        }

        ///<summary>按类型获取组件
        ///</summary>
        ///<typeparam name="TService">组件类型</typeparam>
        ///<returns>返回获取的组件</returns>
        public static TService Resolve<TService>(params Parameter[] parameters)
        {
            using (_container.BeginLifetimeScope())
            {
                return ResolutionExtensions.Resolve<TService>(_container, parameters);
            }
        }

        ///<summary>按key获取组件
        ///</summary>
        ///<typeparam name="TService">组件类型</typeparam>
        ///<param name="serviceKey">枚举类型的Key</param>
        ///<returns>返回获取的组件</returns>
        public static TService ResolveKeyed<TService>(object serviceKey)
        {
            using (_container.BeginLifetimeScope())
            {
                return ResolutionExtensions.ResolveKeyed<TService>(_container, serviceKey);
            }
        }

        ///<summary>按名称获取组件
        ///</summary>
        ///<typeparam name="TService">组件类型</typeparam>
        ///<param name="serviceName">组件名称</param>
        ///<returns>返回获取的组件</returns>
        public static TService ResolveNamed<TService>(string serviceName)
        {
            using (_container.BeginLifetimeScope())
            {
                return ResolutionExtensions.ResolveNamed<TService>(_container, serviceName);
            }
        }
        
    }
}

