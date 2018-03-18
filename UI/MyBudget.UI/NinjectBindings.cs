using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Views;
using Ninject;
using Ninject.Modules;

namespace MyBudget.UI
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindow>().ToSelf().InTransientScope();
            Bind<NavigationPanelViewModel>().ToSelf().InTransientScope();
            Bind<CurrentMonthViewModel>().ToSelf().InTransientScope();
            Bind<AddTransactionViewModel>().ToSelf().InTransientScope();
        }
    }
}
