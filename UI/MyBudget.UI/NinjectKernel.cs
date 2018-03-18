using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI
{
    public static class NinjectKernel
    {
        private static StandardKernel kernel;

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        public static object Get(Type service)
        {
            return kernel.Get(service);
        }

        public static void Initialize(params Assembly[] assemblies)
        {
            if (kernel == null)
            {
                kernel = new StandardKernel();
                kernel.Load(assemblies);
            }
        }
    }
}
