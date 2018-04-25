using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using MyBudget.Spreadsheet;

namespace MyBudget.Repository
{
    public class NinjectRepositoryBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<StorageSchema>().ToConstant(StorageSchema.Load("StorageSchema.xml")).InSingletonScope();
            //Bind<ISpreadsheetOperations>().To<ElementarOperations>().InSingletonScope();
        }
    }
}
