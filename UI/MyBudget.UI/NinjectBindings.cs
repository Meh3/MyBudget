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
            Bind<CurrentMonthView>().ToSelf().InTransientScope();
            Bind<CurrentMonthViewModel>().ToSelf().InTransientScope();
            Bind<AddTransactionView>().ToSelf().InTransientScope();
            Bind<AddTransactionViewModel>().ToSelf().InTransientScope();
            Bind<StatisticsView>().ToSelf().InTransientScope();
            Bind<StatisticsViewModel>().ToSelf().InTransientScope();
            Bind<IDictionary<string, Type>>().ToConstant(new Dictionary<string, Type>
            {
                ["thisMonth"] = typeof(CurrentMonthView),
                ["addNew"] = typeof(AddTransactionView),
                ["statistics"] = typeof(StatisticsView)
            }).InSingletonScope();
        }
    }
}
