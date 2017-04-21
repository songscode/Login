using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Login.Common;
using Login.Common.Caching;
using MicroFramework.Infrastructure.Caching;

namespace Login
{
    public static partial class SiteInitialization
    {
        public static void ApplicationStart()
        {
            //ioc注入
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => new RuntimeMemoryCache()).As<ICache>().SingleInstance();
            IContainer container = containerBuilder.Build();
            DIContainer.RegisterContainer(container);

            foreach (var databaseKey in databaseKeys)
            {
                EnsureDatabase(databaseKey);
                RunMigrations(databaseKey);
            }
        }

        public static void ApplicationEnd()
        {
        }
    }
}
