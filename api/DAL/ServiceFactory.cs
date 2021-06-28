using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DAL.Data;
using Castle.DynamicProxy;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.MicroKernel.Registration;
using System.Reflection;
using MyOrm.Common;
using MyOrm;
using System.Data;

namespace DAL.Business
{
    public static class ServiceFactory
    {
        /// <summary>
        /// 注册ServiceFactory
        /// </summary>
        public static void RegisterServiceFactory()
        {
            Utility.RegisterDbType(typeof(Boolean), DbType.Byte);

            App.Container.Register(Component.For(typeof(SessionManager)).Instance(new SessionManager(Configuration.DefaultConnection)));

            ServiceBase.ServiceContainer.Register(Types.FromAssembly(typeof(CObjectDAO<>).Assembly).BasedOn<ObjectDAOBase>().If(type => !type.IsGenericType).WithServiceAllInterfaces());
            ServiceBase.ServiceContainer.Register(Types.FromAssembly(typeof(IServiceFactory).Assembly).BasedOn<ServiceBase>().If(type => !type.IsGenericType).WithServiceAllInterfaces());

            ProxyGenerator proxy = new ProxyGenerator();
            IServiceFactory serviceFactory = proxy.CreateInterfaceProxyWithoutTarget<IServiceFactory>(new FactoryInterceptor());
            ServiceBase.ServiceContainer.Register(Component.For(typeof(IServiceFactory)).Instance(serviceFactory));

            IServiceFactory serviceFactoryProxy = proxy.CreateInterfaceProxyWithTarget<IServiceFactory>(serviceFactory, new FactoryProxyInterceptor());
            App.Container.Register(Component.For(typeof(IServiceFactory)).Instance(serviceFactoryProxy));
            foreach (PropertyInfo property in typeof(IServiceFactory).GetProperties())
            {
                List<Type> interfaces = new List<Type>(property.PropertyType.GetInterfaces());
                interfaces.Add(property.PropertyType);
                interfaces.Remove(typeof(IEntityService));
                interfaces.Remove(typeof(IEntityViewService));
                App.Container.Register(Component.For(interfaces).Instance(property.GetValue(serviceFactoryProxy, null)));
            }
        }

        public static IServiceFactory Factory
        {
            get
            {
                return App.Container.Resolve<IServiceFactory>();
            }
        }

        public static IEntityService<T> GetEntityService<T>()
        {
            return App.Container.Resolve<IEntityService<T>>();
        }

        public static IEntityViewService<T> GetEntityViewService<T>()
        {
            return App.Container.Resolve<IEntityViewService<T>>();
        }

        public static SessionManager SessionManager
        {
            get
            {
                return App.Container.Resolve<SessionManager>();
            }
        }
    }
}
